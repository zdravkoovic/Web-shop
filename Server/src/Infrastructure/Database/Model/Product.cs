using System.ComponentModel.DataAnnotations;
using WebShop.src.Infrastructure.SQL.Database.Model;

namespace Infrastructure.SQL.Database.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = null!;
        public int StockQuantity { get; set; }
        [MaxLength(3000)]
        public string? Description { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public ICollection<ProductImage> Images { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];
    }
}