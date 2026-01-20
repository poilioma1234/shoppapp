namespace ProductApp.Application.DTOs.Order;

public class OrderCreateDto
{
    public string UserId { get; set; } = string.Empty;
    public string? ShippingAddress { get; set; }
    public string? Notes { get; set; }
    public List<OrderItemCreateDto> OrderItems { get; set; } = new();
}
