using System.ComponentModel.DataAnnotations;

namespace Infrastructure.SQL.Database.Model
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
    }
}