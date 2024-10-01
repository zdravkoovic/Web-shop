using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infrastructure.SQL.Database.Model;

namespace WebShop.src.Infrastructure.SQL.Database.Model;

public class ProductImage
{
    [Key]
    [JsonIgnore]
    public int Id { get; set;}
    public string? Path { get; set; }

    [JsonIgnore]
    public Product Product { get; set; } = null!;
}