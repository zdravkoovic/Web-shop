using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.SQL.Database.Model;

public class Comment
{
    [Key]
    public int Id { get; set;}
    [Required]
    [ForeignKey("UserId")]
    public string? UserId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [StringLength(255, MinimumLength = 10)]
    public string? Text { get; set; }
    [Required]
    [Range(1,6)]
    public int Rate { get; set; }
    public DateTime Timestamp { get; set; }
    public Product Product { get; set; } = null!;
    public ApiUser User { get; set; } = null!;

}