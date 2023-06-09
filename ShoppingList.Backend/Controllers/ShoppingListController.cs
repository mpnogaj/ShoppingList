using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Backend.Db;
using ShoppingList.Backend.DTO;
using ShoppingList.Backend.Models;
using ShoppingList.Backend.Services;

namespace ShoppingList.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShoppingListController : ControllerBase
{
	private readonly ShoppingListDbContext _db;
	private readonly UserInfoService _userInfoService;

	public ShoppingListController(ShoppingListDbContext db, UserInfoService userInfoService)
	{
		_db = db;
		_userInfoService = userInfoService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ShoppingListDTO>>> GetShoppingLists()
	{
		string userGuid = _userInfoService.GetUserGuid(HttpContext.User);
		return await _db.ShoppingLists
			.Include(x => x.Products)
			.Where(x => x.UserGuid == userGuid)
			.Select(shoppingList => new ShoppingListDTO
			{
				Id = shoppingList.Id,
				Name = shoppingList.Name,
				Products = shoppingList.Products.Select(product => product.Name).ToList()
			}).ToListAsync();
	}

	[HttpGet("{listId:int}")]
	public async Task<ActionResult<ShoppingListDTO>> GetShoppingList(int listId)
	{
		var shoppingList = await _db.ShoppingLists
			.Include(x => x.Products)
			.FirstOrDefaultAsync(x => x.Id == listId);
		if (shoppingList == null) return NotFound();

		string userGuid = _userInfoService.GetUserGuid(HttpContext.User);
		if (shoppingList.UserGuid != userGuid) return Unauthorized();

		var shoppingListDto = new ShoppingListDTO
		{
			Id = shoppingList.Id,
			Name = shoppingList.Name,
			Products = shoppingList.Products.Select(x => x.Name).ToList()
		};

		return shoppingListDto;
	}

	[HttpPost]
	public async Task<IActionResult> Create(string listName)
	{
		string loggedUserGuid = _userInfoService.GetUserGuid(HttpContext.User);
		await _db.ShoppingLists.AddAsync(new Models.ShoppingList
		{
			Id = 0,
			UserGuid = loggedUserGuid,
			Name = listName,
			Products = new List<Product>()
		});

		await _db.SaveChangesAsync();

		return Ok();
	}

	[HttpPut]
	public async Task<IActionResult> Update(ShoppingListDTO shoppingListDto)
	{
		var shoppingList = await _db.ShoppingLists
			.Include(x => x.Products)
			.FirstOrDefaultAsync(x => x.Id == shoppingListDto.Id);
		if (shoppingList == null)
			return NotFound();

		var productsOnList = new List<Product>();

		//add new products
		foreach (string productDto in shoppingListDto.Products)
		{
			var product = await _db.Products.FirstOrDefaultAsync(x => x.Name == productDto);
			if (product == null)
			{
				product = new Product
				{
					Id = 0,
					Name = productDto,
				};
				await _db.Products.AddAsync(product);
			}

			productsOnList.Add(product);
		}

		await _db.SaveChangesAsync();

		shoppingList.Name = shoppingListDto.Name;
		shoppingList.Products.Clear();
		foreach (var product in productsOnList) 
			shoppingList.Products.Add(product);
		await _db.SaveChangesAsync();

		return Ok();
	}

	[HttpDelete]
	public async Task<IActionResult> Delete(int id)
	{
		var shoppingList = await _db.ShoppingLists
			.Include(x => x.Products)
			.FirstOrDefaultAsync(x => x.Id == id);
		if (shoppingList == null) return NotFound();

		string userGuid = _userInfoService.GetUserGuid(HttpContext.User);
		if (shoppingList.UserGuid != userGuid) return Unauthorized();

		_db.ShoppingLists.Remove(shoppingList);
		await _db.SaveChangesAsync();

		return Ok();
	}
}