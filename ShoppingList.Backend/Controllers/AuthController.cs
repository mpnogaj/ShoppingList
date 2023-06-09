using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Backend.Db;
using ShoppingList.Backend.DTO;
using ShoppingList.Backend.Services;

namespace ShoppingList.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
	private readonly TokenService _tokenService;
	private readonly IdentityUserDbContext _userDb;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly UserInfoService _userInfoService;
	private readonly IConfiguration _configuration;

	public AuthController(UserManager<IdentityUser> userManager, IdentityUserDbContext userDb,
		TokenService tokenService, IConfiguration configuration, UserInfoService userInfoService)
	{
		_userManager = userManager;
		_userDb = userDb;
		_tokenService = tokenService;
		_configuration = configuration;
		_userInfoService = userInfoService;
	}

	[HttpPost]
	public async Task<IActionResult> Register([FromBody] AuthRequest authRequest)
	{
		var user = new IdentityUser
		{
			UserName = authRequest.Username
		};
		var result = await _userManager.CreateAsync(user, authRequest.Password);

		if (result.Succeeded)
			return Ok();
		return BadRequest(result.Errors.First().Description);
	}

	[HttpGet]
	public async Task<IActionResult> Authenticate([FromQuery] AuthRequest request)
	{
		var managedUser = await _userManager.FindByNameAsync(request.Username);
		if (managedUser == null)
			return Unauthorized();

		if (!await _userManager.CheckPasswordAsync(managedUser, request.Password))
			return Unauthorized();

		var userInDb = await _userDb.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);

		if (userInDb == null)
			return Unauthorized();

		string token = _tokenService.CreateToken(userInDb);

		await _userDb.SaveChangesAsync();

		string? cookieName = _configuration["JwtSettings:TokenCookieName"];
		
		if(cookieName == null)
			throw new NullReferenceException(nameof(cookieName));
		
		HttpContext.Response.Cookies.Append(cookieName, token, new CookieOptions
		{
			Expires = TokenService.GetExpirationTime(),
			HttpOnly = true,
			Secure = true,
			IsEssential = true,
			SameSite = SameSiteMode.None
		});

		return Ok(token);
	}

	[HttpPost]
	public IActionResult Logout()
	{
		string? cookieName = _configuration["JwtSettings:TokenCookieName"];
		
		if(cookieName == null)
			throw new NullReferenceException(nameof(cookieName));
		
		HttpContext.Response.Cookies.Delete(cookieName);
		
		return Ok();
	}
	
	[HttpGet]
	public ActionResult<UserDTO> GetLoggedUser()
	{
		var loggedUser = HttpContext.User;
		return new UserDTO
		{
			UserGuid = _userInfoService.GetUserGuid(loggedUser),
			UserName = _userInfoService.GetUserName(loggedUser)
		};
	}
}

public class AuthRequest
{
	[Required] public string Username { get; set; }

	[Required] public string Password { get; set; }
}