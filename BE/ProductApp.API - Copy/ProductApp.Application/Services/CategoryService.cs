using ProductApp.Application.Interfaces.Repositories;
using ProductApp.Application.Interfaces.Services;
using ProductApp.Domain.Entities;

namespace ProductApp.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Category>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<Category?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
    public async Task AddAsync(Category entity) => await _repository.AddAsync(entity);
    public async Task UpdateAsync(Category entity) => await _repository.UpdateAsync(entity);
    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    public async Task<int> SaveChangesAsync() => await _repository.SaveChangesAsync();
}
