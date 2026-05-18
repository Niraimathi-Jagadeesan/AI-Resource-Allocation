using System.ComponentModel.DataAnnotations;

namespace ResourceAllocation.API.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Admin, Manager, Employee
        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "Employee";
    }
}