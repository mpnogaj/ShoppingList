using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingList.Backend.Db;
using ShoppingList.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
	builder.Services.AddDbContext<ShoppingListDbContext>(
		options => options.UseInMemoryDatabase(nameof(ShoppingListDbContext)));

	builder.Services.AddDbContext<IdentityUserDbContext>(
		options => options.UseInMemoryDatabase(nameof(IdentityUserDbContext)));
}
else
{
	builder.Services.AddDbContext<ShoppingListDbContext>(
		options => options.UseSqlite(builder.Configuration.GetConnectionString(nameof(ShoppingListDbContext))));

	builder.Services.AddDbContext<IdentityUserDbContext>(
		options => options.UseSqlite(builder.Configuration.GetConnectionString(nameof(IdentityUserDbContext))));
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo", Version = "v1" });
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Valid JWT token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] { }
		}
	});
});

string? tokenCookieName = builder.Configuration["JwtSettings:TokenCookieName"];

if (tokenCookieName == null)
	throw new NullReferenceException(nameof(tokenCookieName));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddCookie(x => x.Cookie.Name = tokenCookieName)
	.AddJwtBearer(options =>
	{
		string? issuer = builder.Configuration["JwtSettings:Issuer"];
		string? audience = builder.Configuration["JwtSettings:Audience"];
		string? key = builder.Configuration["JwtSettings:SecretKey"];

		if (key == null)
			throw new NullReferenceException(nameof(key));

		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ClockSkew = TimeSpan.Zero,
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = issuer ?? throw new NullReferenceException(nameof(issuer)),
			ValidAudience = audience ?? throw new NullReferenceException(nameof(audience)),
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(key)
			)
		};
		options.Events = new JwtBearerEvents()
		{
			OnMessageReceived = (ctx) =>
			{
				ctx.Token ??= ctx.Request.Cookies[tokenCookieName];
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddIdentityCore<IdentityUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	options.User.RequireUniqueEmail = false;
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 6;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<IdentityUserDbContext>();

builder.Services.AddScoped<TokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();