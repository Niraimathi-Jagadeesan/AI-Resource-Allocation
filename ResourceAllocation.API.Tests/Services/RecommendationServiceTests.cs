using ResourceAllocation.API.Models;
using ResourceAllocation.API.Services;
using Xunit;

namespace ResourceAllocation.API.Tests.Services
{
    public class RecommendationServiceTests
    {
        private readonly RecommendationService _service;

        public RecommendationServiceTests()
        {
            _service = new RecommendationService();
        }

        [Fact]
        public void RecommendEmployees_ShouldReturnOnlyEligibleEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "John",
                    Skills = ".NET,Angular,SQL",
                    Experience = 6,
                    PerformanceScore = 95,
                    CurrentAllocationPercent = 20,
                    IsAvailable = true
                },

                new Employee
                {
                    Id = 2,
                    Name = "David",
                    Skills = "Java,Python",
                    Experience = 5,
                    PerformanceScore = 90,
                    CurrentAllocationPercent = 10,
                    IsAvailable = true
                },

                new Employee
                {
                    Id = 3,
                    Name = "Priya",
                    Skills = ".NET,Azure",
                    Experience = 8,
                    PerformanceScore = 93,
                    CurrentAllocationPercent = 40,
                    IsAvailable = false
                }
            };

            var project = new Project
            {
                Id = 1,
                ProjectName = "AI Resource Allocation",
                RequiredSkills = ".NET,Angular",
                MinimumExperience = 5
            };

            // Act
            var result = _service.RecommendEmployees(
                employees,
                project);

            // Assert
            Assert.Single(result);
            Assert.Equal("John", result[0].EmployeeName);
        }

        [Fact]
        public void RecommendEmployees_ShouldSortByFinalScoreDescending()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "John",
                    Skills = ".NET,Angular",
                    Experience = 6,
                    PerformanceScore = 95,
                    CurrentAllocationPercent = 20,
                    IsAvailable = true
                },

                new Employee
                {
                    Id = 2,
                    Name = "Sara",
                    Skills = ".NET,Angular,AI",
                    Experience = 10,
                    PerformanceScore = 98,
                    CurrentAllocationPercent = 10,
                    IsAvailable = true
                }
            };

            var project = new Project
            {
                RequiredSkills = ".NET,Angular",
                MinimumExperience = 5
            };

            // Act
            var result = _service.RecommendEmployees(
                employees,
                project);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Sara", result[0].EmployeeName);
            Assert.True(result[0].FinalScore >= result[1].FinalScore);
        }

        [Fact]
        public void RecommendEmployees_ShouldPopulateReason()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "John",
                    Skills = ".NET,Angular",
                    Experience = 6,
                    PerformanceScore = 95,
                    CurrentAllocationPercent = 20,
                    IsAvailable = true
                }
            };

            var project = new Project
            {
                RequiredSkills = ".NET",
                MinimumExperience = 5
            };

            // Act
            var result = _service.RecommendEmployees(
                employees,
                project);

            // Assert
            Assert.Single(result);
            Assert.False(string.IsNullOrWhiteSpace(result[0].Reason));
            Assert.Contains("Skill Match", result[0].Reason);
        }

        [Fact]
        public void RecommendEmployees_ShouldExcludeFullyAllocatedEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "John",
                    Skills = ".NET,Angular",
                    Experience = 6,
                    PerformanceScore = 95,
                    CurrentAllocationPercent = 100,
                    IsAvailable = true
                }
            };

            var project = new Project
            {
                RequiredSkills = ".NET",
                MinimumExperience = 5
            };

            // Act
            var result = _service.RecommendEmployees(
                employees,
                project);

            // Assert
            Assert.Empty(result);
        }
    }
}