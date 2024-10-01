using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.SQL.Database.Model;

public class ApiUser : IdentityUser
{
    [MaxLength(20)]
    public required string FirstName { get; set;}
    [MaxLength(20)]
    public required string LastName { get; set; }
    [MaxLength(50)]
    public string? Address { get; set; }
}