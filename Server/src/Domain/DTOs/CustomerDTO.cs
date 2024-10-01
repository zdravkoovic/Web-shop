using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class CustomerDto
    {
        public string? Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set;}
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
    }
}