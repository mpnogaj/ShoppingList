using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Backend.Db;
using ShoppingList.Backend.Services;

namespace ShoppingList.Backend.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
	private readonly TokenService _tokenService;
	private readonly IdentityUserDbContext _userDb;
	private readonly UserManager<IdentityUser> _userManager;

	public AuthController(UserManager<IdentityUser> userManager, IdentityUserDbContext userDb,
		TokenService tokenService)
	{
		_userManager = userManager;
		_userDb = userDb;
		_tokenService = tokenService;
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

		var token = _tokenService.CreateToken(userInDb);

		await _userDb.SaveChangesAsync();

		return Ok(token);
	}
}

public class AuthRequest
{
	[Required] public string Username { get; set; }

	[Required] public string Password { get; set; }
}