using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Backend.Models;

[PrimaryKey(nameof(Id))]
public class Product
{
	public int Id { get; set; }
	public string Name { get; set; }

	public List<ShoppingList> ShoppingLists { get; set; }
}