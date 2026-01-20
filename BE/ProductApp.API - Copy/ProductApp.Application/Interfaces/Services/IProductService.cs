using ProductApp.Domain.Entities;

namespace ProductApp.Application.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);      
    Task AddAsync(Product entity);
    Task UpdateAsync(Product entity);         
    Task DeleteAsync(int id);                
    Task<int> SaveChangesAsync();
}