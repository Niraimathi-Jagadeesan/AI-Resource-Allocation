using Moq;
using ResourceAllocation.API.Models;
using ResourceAllocation.API.Services;
using Xunit;

namespace ResourceAllocation.API.Tests.Services
{
    public class RiskAnalysisServiceTests
    {
        private readonly RiskAnalysisService _service;

        public RiskAnalysisServiceTests()
        {
            // Create a mock LLM service
            var llmServiceMock = new Mock<ILLMService>();

            // Mock AskAsync() to return a predictable AI response
            llmServiceMock
                .Setup(x => x.AskAsync(It.IsAny<string>()))
                .ReturnsAsync("""
                    {
                      "riskScore": 85,
                      "riskLevel": "High",
                      "summary": "This project has a high risk due to high priority and complexity.",
                      "riskFactors": [
                        "Project priority is very high.",
                        "Project complexity is high."
                      ],
                      "recommendations": [
                        "Assign senior developers.",
                        "Increase monitoring frequency."
                      ]
                    }
                    """);

            // Inject mocked LLM service
            _service = new RiskAnalysisService(llmServiceMock.Object);
        }

        [Fact]
        public async Task Analyze_ShouldReturnHighRisk_ForComplexProject()
        {
            // Arrange
            var project = new Project
            {
                ProjectName = "AI Platform",
                Priority = 9,
                MinimumExperience = 10,
                Complexity = "High",
                RequiredSkills = ".NET,Angular,Azure,AI"
            };

            // Act
            var result = await _service.AnalyzeAsync(project);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("High", result.RiskLevel);
            Assert.Equal(85, result.RiskScore);
            Assert.NotEmpty(result.RiskFactors);
            Assert.NotEmpty(result.Recommendations);
            Assert.False(string.IsNullOrWhiteSpace(result.Summary));
        }

        [Fact]
        public async Task Analyze_ShouldReturnRiskFactors()
        {
            // Arrange
            var project = new Project
            {
                ProjectName = "CRM Project",
                Priority = 8,
                MinimumExperience = 7,
                Complexity = "High",
                RequiredSkills = ".NET,SQL,Azure"
            };

            // Act
            var result = await _service.AnalyzeAsync(project);

            // Assert
            Assert.NotNull(result.RiskFactors);
            Assert.NotEmpty(result.RiskFactors);
            Assert.Contains(
                "Project priority is very high.",
                result.RiskFactors);
        }

        [Fact]
        public async Task Analyze_ShouldReturnRecommendations()
        {
            // Arrange
            var project = new Project
            {
                ProjectName = "ERP System",
                Priority = 9,
                MinimumExperience = 8,
                Complexity = "High",
                RequiredSkills = ".NET,Angular,AI"
            };

            // Act
            var result = await _service.AnalyzeAsync(project);

            // Assert
            Assert.NotNull(result.Recommendations);
            Assert.NotEmpty(result.Recommendations);
            Assert.Contains(
                "Assign senior developers.",
                result.Recommendations);
        }

        [Fact]
        public async Task Analyze_ShouldReturnSummary()
        {
            // Arrange
            var project = new Project
            {
                ProjectName = "Analytics Dashboard",
                Priority = 7,
                MinimumExperience = 5,
                Complexity = "Medium",
                RequiredSkills = ".NET,Angular"
            };

            // Act
            var result = await _service.AnalyzeAsync(project);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(result.Summary));
        }
    }
}