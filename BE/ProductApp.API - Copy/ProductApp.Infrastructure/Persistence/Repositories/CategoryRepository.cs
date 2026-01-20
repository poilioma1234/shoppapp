using Microsoft.EntityFrameworkCore;
using ProductApp.Application.Interfaces.Repositories;
using ProductApp.Domain.Entities;
using ProductApp.Infrastructure.Persistence;

namespace ProductApp.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductAppDbContext _context;

    public CategoryRepository(ProductAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync() => await _context.Categories.ToListAsync();
    public async Task<Category?> GetByIdAsync(int id) => await _context.Categories.FindAsync(id);
    public async Task AddAsync(Category entity) => await _context.Categories.AddAsync(entity);

    public async Task UpdateAsync(Category entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category != null) _context.Categories.Remove(category);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
