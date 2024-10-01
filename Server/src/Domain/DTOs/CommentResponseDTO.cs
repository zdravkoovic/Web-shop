using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebShop.src.Domain.DTOs;

public class CommentResponseDTO {
    public required int Rate { get; set; }
    public required string Text { get; set; }
    public required DateTime Timestamp { get; set; }
    public required byte[] AvatarPicture { get; set; }
    public required string Username { get; set; }
}