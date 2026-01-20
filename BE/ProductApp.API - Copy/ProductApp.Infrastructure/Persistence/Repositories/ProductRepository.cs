using Microsoft.EntityFrameworkCore;
using ProductApp.Application.Interfaces.Repositories;
using ProductApp.Domain.Entities;
using ProductApp.Infrastructure.Persistence;

namespace ProductApp.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductAppDbContext _context;

    public ProductRepository(ProductAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => 
        await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
            
    public async Task<Product?> GetByIdAsync(int id) => 
        await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    public async Task AddAsync(Product entity) => await _context.Products.AddAsync(entity);

    public async Task UpdateAsync(Product entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product != null) _context.Products.Remove(product);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}