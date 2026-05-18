using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ResourceAllocation.API.Controllers;
using ResourceAllocation.API.Data;
using ResourceAllocation.API.Models;
using ResourceAllocation.API.Services;
using Xunit;

namespace ResourceAllocation.API.Tests.Controllers
{
    public class RecommendationControllerTests
    {
        private readonly AppDbContext _context;
        private readonly RecommendationController _controller;

        public RecommendationControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            SeedData(_context);

            var recommendationService = new RecommendationService();

            var llmMock = new Mock<ILLMService>();
            llmMock
                .Setup(x => x.AskAsync(It.IsAny<string>()))
                .ReturnsAsync("AI-generated explanation.");

            _controller = new RecommendationController(
                _context,
                recommendationService,
                llmMock.Object);
        }

        private void SeedData(AppDbContext context)
        {
            context.Projects.Add(new Project
            {
                Id = 1,
                ProjectName = "AI Project",
                RequiredSkills = ".NET,Angular",
                MinimumExperience = 5
            });

            context.Employees.Add(new Employee
            {
                Id = 1,
                Name = "John",
                Skills = ".NET,Angular,SQL",
                Experience = 6,
                PerformanceScore = 95,
                CurrentAllocationPercent = 20,
                IsAvailable = true
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenProjectExists()
        {
            // Act
            var actionResult = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(
                actionResult.Result);

            var recommendations =
                Assert.IsAssignableFrom<List<EmployeeRecommendation>>(
                    okResult.Value);

            Assert.Single(recommendations);
            Assert.Equal("John", recommendations[0].EmployeeName);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenProjectDoesNotExist()
        {
            // Act
            var actionResult = await _controller.Get(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(
                actionResult.Result);
        }

        [Fact]
        public async Task Get_ShouldPopulateAIReason()
        {
            // Act
            var actionResult = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(
                actionResult.Result);

            var recommendations =
                Assert.IsAssignableFrom<List<EmployeeRecommendation>>(
                    okResult.Value);

            Assert.Equal(
                "AI-generated explanation.",
                recommendations[0].Reason);
        }
    }
}