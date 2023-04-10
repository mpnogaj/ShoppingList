using Microsoft.EntityFrameworkCore;
using ShoppingList.Backend.Models;

namespace ShoppingList.Backend.Db;

public class ShoppingListDbContext : DbContext
{ 
    public DbSet<Models.ShoppingList> ShoppingLists { get; set; }
    public DbSet<Product> Products { get; set; }

    public ShoppingListDbContext(DbContextOptions<ShoppingListDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.ShoppingList>()
            .HasMany(e => e.Products)
            .WithMany(e => e.ShoppingLists);
    }
}