namespace ProductApp.Application.DTOs.Order;

public class OrderUpdateDto
{
    public string Status { get; set; } = string.Empty;
    public string? ShippingAddress { get; set; }
    public string? Notes { get; set; }
}
