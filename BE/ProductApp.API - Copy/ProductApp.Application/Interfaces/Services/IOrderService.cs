using ProductApp.Domain.Entities;

namespace ProductApp.Application.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
    Task<Order> CreateOrderAsync(Order order, IEnumerable<OrderItem> orderItems);
    Task UpdateAsync(Order entity);
    Task DeleteAsync(int id);
    Task<int> SaveChangesAsync();
}
