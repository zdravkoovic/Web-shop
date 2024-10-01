namespace WebShop.src.Domain.DTOs;

public class AvatarImageDTO
{
    public List<IFormFile> Files { get; set; } = [];
    public string? Param { get; set; }
}