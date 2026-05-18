using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ResourceAllocation.API.Controllers;
using ResourceAllocation.API.Data;
using ResourceAllocation.API.DTOs;
using ResourceAllocation.API.Models;
using ResourceAllocation.API.Services;
using Xunit;

namespace ResourceAllocation.API.Tests.Controllers
{
    public class RiskControllerTests
    {
        private readonly AppDbContext _context;
        private readonly RiskController _controller;

        public RiskControllerTests()
        {
            // Arrange DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            SeedData(_context);

            // Mock LLM service
            var llmMock = new Mock<ILLMService>();
            llmMock
                .Setup(x => x.AskAsync(It.IsAny<string>()))
                .ReturnsAsync("""
                {
                  "riskLevel": "High",
                  "riskScore": 85,
                  "summary": "This project has a high delivery risk.",
                  "riskFactors": [
                    "High project complexity",
                    "Requires multiple specialized skills"
                  ],
                  "recommendations": [
                    "Assign senior engineers",
                    "Increase monitoring"
                  ]
                }
                """);

            // Create service
            var riskService = new RiskAnalysisService(
                llmMock.Object);

            // Create controller
            _controller = new RiskController(
                _context,
                riskService);
        }

        private void SeedData(AppDbContext context)
        {
            context.Projects.Add(new Project
            {
                Id = 1,
                ProjectName = "AI Platform",
                Priority = 9,
                Complexity = "High",
                MinimumExperience = 8,
                RequiredSkills = ".NET,Angular,Azure,OpenAI"
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task Analyze_ShouldReturnOk_WhenProjectExists()
        {
            // Act
            var actionResult = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(
                actionResult);

            var response =
                Assert.IsType<RiskAnalysisResponse>(
                    okResult.Value);

            Assert.Equal("High", response.RiskLevel);
            Assert.Equal(85, response.RiskScore);
        }

        [Fact]
        public async Task Analyze_ShouldReturnNotFound_WhenProjectDoesNotExist()
        {
            // Act
            var actionResult = await _controller.Get(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(
                actionResult);
        }

        [Fact]
        public async Task Analyze_ShouldReturnRiskFactors()
        {
            // Act
            var actionResult = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(
                actionResult);

            var response =
                Assert.IsType<RiskAnalysisResponse>(
                    okResult.Value);

            Assert.NotEmpty(response.RiskFactors);
            Assert.Contains(
                "High project complexity",
                response.RiskFactors);
        }

        [Fact]
        public async Task Analyze_ShouldReturnRecommendations()
        {
            // Act
            var actionResult = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(
                actionResult);

            var response =
                Assert.IsType<RiskAnalysisResponse>(
                    okResult.Value);

            Assert.NotEmpty(response.Recommendations);
            Assert.Contains(
                "Assign senior engineers",
                response.Recommendations);
        }

        [Fact]
        public async Task Analyze_ShouldReturnSummary()
        {
            // Act
            var actionResult = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(
                actionResult);

            var response =
                Assert.IsType<RiskAnalysisResponse>(
                    okResult.Value);

            Assert.Contains("high delivery risk", response.Summary);
        }
    }
}