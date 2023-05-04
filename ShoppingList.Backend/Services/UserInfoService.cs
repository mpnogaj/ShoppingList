using System.Security.Claims;

namespace ShoppingList.Backend.Services;

public class UserInfoService
{
	public string GetUserName(ClaimsPrincipal user)
	{
		var claims = user.Claims;
		var nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

		if (nameClaim == null)
			throw new NullReferenceException(nameof(nameClaim));

		return nameClaim.Value;
	}

	public string GetUserGuid(ClaimsPrincipal user)
	{
		var claims = user.Claims;
		var idClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

		if (idClaim == null)
			throw new NullReferenceException(nameof(idClaim));

		return idClaim.Value;
	}
}