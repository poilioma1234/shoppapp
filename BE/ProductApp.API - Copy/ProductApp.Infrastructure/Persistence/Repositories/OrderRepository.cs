using Microsoft.EntityFrameworkCore;
using ProductApp.Application.Interfaces.Repositories;
using ProductApp.Domain.Entities;
using ProductApp.Infrastructure.Persistence;

namespace ProductApp.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ProductAppDbContext _context;

    public OrderRepository(ProductAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync() => 
        await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .ToListAsync();

    public async Task<Order?> GetByIdAsync(int id) => 
        await _context.Orders.FindAsync(id);

    public async Task<Order?> GetByIdWithItemsAsync(int id) => 
        await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId) => 
        await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .ToListAsync();

    public async Task AddAsync(Order entity) => await _context.Orders.AddAsync(entity);

    public async Task UpdateAsync(Order entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var order = await GetByIdAsync(id);
        if (order != null) _context.Orders.Remove(order);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
