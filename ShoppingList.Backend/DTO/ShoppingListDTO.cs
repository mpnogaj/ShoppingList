namespace ShoppingList.Backend.DTO;

public class ShoppingListDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public List<ProductDTO> Products { get; set; }
}