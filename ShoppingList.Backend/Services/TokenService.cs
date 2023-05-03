using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ShoppingList.Backend.Services;

public class TokenService
{
	private static readonly TimeSpan ExpirationTime = TimeSpan.FromMinutes(30); 
	private readonly IConfiguration _configuration;

	public TokenService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string CreateToken(IdentityUser user)
	{
		var token = CreateJwt(CreateClaims(user), CreateCredentials());
		var tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.WriteToken(token);
	}

	private JwtSecurityToken CreateJwt(IEnumerable<Claim> claims, SigningCredentials credentials)
	{
		string? issuer = _configuration["JwtSettings:Issuer"];
		string? audience = _configuration["JwtSettings:Audience"];

		if (issuer == null || audience == null)
			throw new NullReferenceException(issuer == null ? nameof(issuer) : nameof(audience));

		return new JwtSecurityToken(issuer, audience, claims, signingCredentials: credentials,
			expires: GetExpirationTime());
	}

	public static DateTime GetExpirationTime() => DateTime.Now.Add(ExpirationTime);

	private static IEnumerable<Claim> CreateClaims(IdentityUser user)
	{
		try
		{
			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, "TokenForShoppingListApp"),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
				new(ClaimTypes.NameIdentifier, user.Id),
				new(ClaimTypes.Name, user.UserName ?? throw new ArgumentException())
			};
			return claims;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	private SigningCredentials CreateCredentials()
	{
		string? key = _configuration["JwtSettings:SecretKey"];
		if (key == null)
			throw new NullReferenceException(nameof(key));
		var bytes = Encoding.UTF8.GetBytes(key).ToArray();
		return new SigningCredentials(new SymmetricSecurityKey(bytes),
			SecurityAlgorithms.HmacSha256);
	}
}