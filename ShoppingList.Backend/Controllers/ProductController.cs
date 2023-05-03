using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Backend.Db;
using ShoppingList.Backend.DTO;
using ShoppingList.Backend.Models;

namespace ShoppingList.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
	private readonly ShoppingListDbContext _db;

	public ProductController(ShoppingListDbContext db)
	{
		_db = db;
	}

	[HttpGet]
	[Route("products")]
	public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
	{
		return await _db.Products.Select(x => new ProductDTO
		{
			Id = x.Id,
			Name = x.Name
		}).ToListAsync();
	}

	[HttpPost]
	public async Task<IActionResult> CreateProduct(ProductDTO productDto)
	{
		_db.Products.Add(new Product
		{
			Id = 0,
			Name = productDto.Name
		});
		await _db.SaveChangesAsync();
		return Ok();
	}

	[HttpPut]
	public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO productDto)
	{
		var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == productDto.Id);
		if (product == null)
			return NotFound();

		product.Name = productDto.Name;
		await _db.SaveChangesAsync();

		return Ok();
	}

	[HttpGet]
	public async Task<ActionResult<ProductDTO>> GetProduct([FromQuery] int id)
	{
		var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
		if (product == null)
			return NotFound();
		return new ProductDTO
		{
			Id = product.Id,
			Name = product.Name
		};
	}
}