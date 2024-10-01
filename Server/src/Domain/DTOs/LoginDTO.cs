using System.ComponentModel.DataAnnotations;

namespace WebShop.src.Domain.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "Username is required!")]
    [MaxLength(100)]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [MaxLength(100)]
    public string? Password { get; set; }
}