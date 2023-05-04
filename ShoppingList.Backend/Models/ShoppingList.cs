using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Backend.Models;

[PrimaryKey(nameof(Id))]
public class ShoppingList
{
	public int Id { get; set; }
	public string UserGuid { get; set; }
	public string Name { get; set; }
	public List<Product> Products { get; set; }
}