using System.ComponentModel.DataAnnotations;

namespace ResourceAllocation.API.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Example: ".NET,Angular,SQL"
        [Required]
        [MaxLength(500)]
        public string Skills { get; set; } = string.Empty;

        public int Experience { get; set; }

        public bool IsAvailable { get; set; } = true;

        public double PerformanceScore { get; set; }

        // Current utilization percentage (0 to 100)
        public int CurrentAllocationPercent { get; set; }

        // Optional metadata
        [MaxLength(100)]
        public string? PrimaryRole { get; set; }

        public DateTime? AvailableFrom { get; set; }

        public double SuccessRate { get; set; }
    }
}