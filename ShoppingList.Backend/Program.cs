using Microsoft.EntityFrameworkCore;
using ShoppingList.Backend.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ShoppingListDbContext>(
        options => options.UseInMemoryDatabase(nameof(ShoppingListDbContext)));
}
else
{
    builder.Services.AddDbContext<ShoppingListDbContext>(
        options => options.UseSqlite(builder.Configuration.GetConnectionString(nameof(ShoppingListDbContext))));
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();