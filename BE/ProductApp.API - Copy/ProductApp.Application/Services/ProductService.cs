using ProductApp.Application.Interfaces.Repositories;
using ProductApp.Application.Interfaces.Services;
using ProductApp.Domain.Entities;

namespace ProductApp.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<Product?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
    public async Task AddAsync(Product entity) => await _repository.AddAsync(entity);
    public async Task UpdateAsync(Product entity) => await _repository.UpdateAsync(entity);
    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    public async Task<int> SaveChangesAsync() => await _repository.SaveChangesAsync();
}