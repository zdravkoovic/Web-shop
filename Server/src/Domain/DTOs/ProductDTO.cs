namespace WebShop.src.Domain.DTOs;

public class ProductDTO
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public int StockQuantity { get; set; }
}