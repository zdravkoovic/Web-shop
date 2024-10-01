namespace WebShop.src.Domain.DTOs;

public class CommentHubDTO
{
    public string? Text { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Username { get; set; }
    public int Rate { get; set; }
}