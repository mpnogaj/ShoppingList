using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ShoppingList.Backend.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PingController : Controller
{
	[HttpGet]
	public IActionResult Ping()
	{
		return Ok("Pong");
	}

	[HttpGet]
	[Authorize]
	public IActionResult AuthPing()
	{
		return Ok("Pong");
	}
}