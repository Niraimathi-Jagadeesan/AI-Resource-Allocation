using System.ComponentModel.DataAnnotations;

namespace ResourceAllocation.API.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectName { get; set; } = string.Empty;

        // Example: ".NET,Angular"
        [Required]
        [MaxLength(500)]
        public string RequiredSkills { get; set; } = string.Empty;

        public int MinimumExperience { get; set; }

        // Optional fields
        public int Priority { get; set; }

        [MaxLength(50)]
        public string? Complexity { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}