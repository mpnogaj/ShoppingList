using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Backend.Db;
using ShoppingList.Backend.DTO;
using ShoppingList.Backend.Models;

namespace ShoppingList.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingListController : ControllerBase
{
	private readonly ShoppingListDbContext _db;

	public ShoppingListController(ShoppingListDbContext db)
	{
		_db = db;
	}

	[HttpGet]
	[Authorize]
	public async Task<ActionResult<IEnumerable<ShoppingListDTO>>> GetShoppingLists()
	{
		return await _db.ShoppingLists
			.Include(x => x.Products)
			.Select(shoppingList => new ShoppingListDTO
			{
				Id = shoppingList.Id,
				Name = shoppingList.Name,
				Products = shoppingList.Products.Select(product => new ProductDTO
				{
					Id = product.Id,
					Name = product.Name
				}).ToList()
			}).ToListAsync();
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<ShoppingListDTO>> GetShoppingList(int id)
	{
		var shoppingList = await _db.ShoppingLists
			.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
		if (shoppingList == null) return NotFound();

		var shoppingListDto = new ShoppingListDTO
		{
			Id = shoppingList.Id,
			Name = shoppingList.Name,
			Products = shoppingList.Products.Select(x => new ProductDTO
			{
				Id = x.Id,
				Name = x.Name
			}).ToList()
		};

		return shoppingListDto;
	}

	[HttpPost]
	public async Task<IActionResult> Create(ShoppingListDTO shoppingListDto)
	{
		await _db.ShoppingLists.AddAsync(new Models.ShoppingList
		{
			Id = 0,
			Name = shoppingListDto.Name,
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

		//add and update products
		foreach (var productDto in shoppingListDto.Products)
		{
			var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == productDto.Id);
			if (product == null)
			{
				product = new Product
				{
					Id = 0,
					Name = productDto.Name,
					ShoppingLists = new List<Models.ShoppingList>()
				};
				await _db.Products.AddAsync(product);
			}
			else
			{
				product.Name = productDto.Name;
			}

			productsOnList.Add(product);
		}

		await _db.SaveChangesAsync();

		shoppingList.Name = shoppingListDto.Name;
		shoppingList.Products.Clear();
		foreach (var product in productsOnList) shoppingList.Products.Add(product);
		await _db.SaveChangesAsync();

		return Ok();
	}
}