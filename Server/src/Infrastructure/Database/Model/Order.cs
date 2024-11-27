using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.SQL.Database.Model
{
    public class Order
    {

        [Key]
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Unknown";
        
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public required ApiUser Customer { get; set; }
    }
}