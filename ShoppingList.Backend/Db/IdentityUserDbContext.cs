using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Backend.Db;

public class IdentityUserDbContext : IdentityUserContext<IdentityUser>
{
	public IdentityUserDbContext(DbContextOptions<IdentityUserDbContext> options) : base(options)
	{
	}
}