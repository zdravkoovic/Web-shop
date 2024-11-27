using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.src.Infrastructure.SQL.Database.Model;

public class AvatarImage
{
    [Key]
    public int Id { get; set; }
    public byte[]? Content { get; set; }
    [Required]
    [ForeignKey("UserId")]
    public string? UserId { get; set; }
}