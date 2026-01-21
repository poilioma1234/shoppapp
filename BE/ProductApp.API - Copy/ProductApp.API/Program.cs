using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductApp.API.Middleware;
using ProductApp.Application.Interfaces.Repositories;
using ProductApp.Application.Interfaces.Services;
using ProductApp.Application.Mappings;
using ProductApp.Application.Services;
using ProductApp.Domain.Entities;
using ProductApp.Infrastructure.Persistence;
using ProductApp.Infrastructure.Persistence.Repositories;
using ProductApp.Infrastructure.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ProductAppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    
    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ProductAppDbContext>()
.AddDefaultTokenProviders();

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddOpenApi();               
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Product Management API v1");

        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Enable static files to serve uploaded images
app.UseStaticFiles();

// Error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();