using ProductApp.Application.Interfaces.Repositories;
using ProductApp.Application.Interfaces.Services;
using ProductApp.Domain.Entities;

namespace ProductApp.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Order>> GetAllAsync() => await _orderRepository.GetAllAsync();
    
    public async Task<Order?> GetByIdAsync(int id) => await _orderRepository.GetByIdWithItemsAsync(id);
    
    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId) => 
        await _orderRepository.GetByUserIdAsync(userId);

    public async Task<Order> CreateOrderAsync(Order order, IEnumerable<OrderItem> orderItems)
    {
        // Calculate total amount
        decimal totalAmount = 0;
        
        foreach (var item in orderItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new ArgumentException($"Sản phẩm với ID {item.ProductId} không tồn tại");

            if (product.StockQuantity < item.Quantity)
                throw new InvalidOperationException($"Không đủ hàng cho sản phẩm {product.Name}. Số lượng còn lại: {product.StockQuantity}");

            item.UnitPrice = product.Price;
            item.SubTotal = item.Quantity * item.UnitPrice;
            totalAmount += item.SubTotal;
            
            // Update stock
            product.StockQuantity -= item.Quantity;
        }

        order.TotalAmount = totalAmount;
        order.OrderItems = orderItems.ToList();

        await _orderRepository.AddAsync(order);
        return order;
    }

    public async Task UpdateAsync(Order entity) => await _orderRepository.UpdateAsync(entity);
    public async Task DeleteAsync(int id) => await _orderRepository.DeleteAsync(id);
    public async Task<int> SaveChangesAsync() => await _orderRepository.SaveChangesAsync();
}
