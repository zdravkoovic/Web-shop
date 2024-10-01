using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.SQL.Database.Model;

public class ChatMessage
{
    [Key]
    public int Id { get; set;}

    [Required]
    [ForeignKey("SenderId")]
    public string? SenderId { get; set; }
    [Required]
    [StringLength(50)]
    public string? SenderName { get; set; }
    [Required]
    [ForeignKey("RecipientId")]
    public string? RecipientId { get; set; }
    [Required]
    [StringLength(50)]
    public string? RecipientName { get; set; }
    [Required]
    [StringLength(255, MinimumLength = 10)]
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; }

    
    
}