using Microsoft.EntityFrameworkCore;
using ResourceAllocation.API.Data;
using ResourceAllocation.API.Models;

namespace ResourceAllocation.API.Services
{
    public class DbSeeder
    {
        private readonly AppDbContext _context;

        public DbSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Apply migrations automatically
            await _context.Database.MigrateAsync();

            //
            // Seed Users
            //
            if (!await _context.Users.AnyAsync())
            {
                _context.Users.AddRange(
                    new AppUser
                    {
                        Username = "admin",
                        Password = "Admin@123",
                        Role = "Admin"
                    },
                    new AppUser
                    {
                        Username = "manager",
                        Password = "Manager@123",
                        Role = "Manager"
                    },
                    new AppUser
                    {
                        Username = "employee",
                        Password = "Employee@123",
                        Role = "Employee"
                    });
            }

            //
            // Seed Employees
            //
            if (!await _context.Employees.AnyAsync())
            {
                _context.Employees.AddRange(
                    new Employee
                    {
                        Name = "John",
                        Skills = ".NET,Angular,SQL",
                        Experience = 6,
                        IsAvailable = true,
                        PerformanceScore = 95,
                        CurrentAllocationPercent = 20,
                        PrimaryRole = "Full Stack Developer",
                        SuccessRate = 92
                    },
                    new Employee
                    {
                        Name = "Priya",
                        Skills = ".NET,Azure,React",
                        Experience = 8,
                        IsAvailable = true,
                        PerformanceScore = 92,
                        CurrentAllocationPercent = 40,
                        PrimaryRole = "Cloud Engineer",
                        SuccessRate = 94
                    },
                    new Employee
                    {
                        Name = "David",
                        Skills = "Java,Angular,SQL",
                        Experience = 5,
                        IsAvailable = true,
                        PerformanceScore = 88,
                        CurrentAllocationPercent = 70,
                        PrimaryRole = "Software Engineer",
                        SuccessRate = 85
                    },
                    new Employee
                    {
                        Name = "Sara",
                        Skills = ".NET,Angular,AI,Azure",
                        Experience = 10,
                        IsAvailable = true,
                        PerformanceScore = 98,
                        CurrentAllocationPercent = 10,
                        PrimaryRole = "Solution Architect",
                        SuccessRate = 97
                    });
            }

            //
            // Seed Projects
            //
            if (!await _context.Projects.AnyAsync())
            {
                _context.Projects.AddRange(
                    new Project
                    {
                        ProjectName = "AI Resource Allocation",
                        RequiredSkills = ".NET,Angular",
                        MinimumExperience = 5,
                        Priority = 9,
                        Complexity = "High",
                        StartDate = DateTime.UtcNow
                    },
                    new Project
                    {
                        ProjectName = "Azure Migration",
                        RequiredSkills = ".NET,Azure",
                        MinimumExperience = 6,
                        Priority = 7,
                        Complexity = "Medium",
                        StartDate = DateTime.UtcNow
                    });
            }

            await _context.SaveChangesAsync();
        }
    }
}