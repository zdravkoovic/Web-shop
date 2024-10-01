using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infrastructure.SQL.Database.Model;

namespace WebShop.src.Domain.DTOs;

public class CommentRequestDTO
{
    [Required]
    public required int ProductId { get; set;}
    [Required]
    [MaxLength(250)]
    [MinLength(2)]
    public required string Comment { get; set;}
    [Required]
    [Range(1,5)]
    public required int Rate { get; set; }
    [JsonIgnore]
    public DateTime? Created { get; set;} = DateTime.UtcNow;
    [JsonIgnore]
    public ApiUser? User { get; set; } = null!;
}