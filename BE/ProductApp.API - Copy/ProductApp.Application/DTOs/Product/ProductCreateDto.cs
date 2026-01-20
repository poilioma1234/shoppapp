namespace ProductApp.Application.DTOs.Product;

public class ProductCreateDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int StockQuantity { get; set; } = 0;
    public int? CategoryId { get; set; }
}