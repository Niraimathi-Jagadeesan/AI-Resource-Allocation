using FluentAssertions;
using ResourceAllocation.API.Helpers;
using Xunit;

namespace ResourceAllocation.API.Tests.Services
{
    public class SkillHelperTests
    {
        [Fact]
        public void CalculateSkillMatchPercentage_ShouldReturn100_WhenAllSkillsMatch()
        {
            // Arrange
            string employeeSkills = ".NET,Angular,SQL";
            string requiredSkills = ".NET,SQL";

            // Act
            double result = SkillHelper.CalculateSkillMatchPercentage(
                employeeSkills,
                requiredSkills);

            // Assert
            result.Should().Be(100);
        }

        [Fact]
        public void CalculateSkillMatchPercentage_ShouldReturn50_WhenHalfSkillsMatch()
        {
            // Arrange
            string employeeSkills = ".NET,Angular";
            string requiredSkills = ".NET,SQL";

            // Act
            double result = SkillHelper.CalculateSkillMatchPercentage(
                employeeSkills,
                requiredSkills);

            // Assert
            result.Should().Be(50);
        }

        [Fact]
        public void CalculateSkillMatchPercentage_ShouldReturn0_WhenNoSkillsMatch()
        {
            // Arrange
            string employeeSkills = "Java,Python";
            string requiredSkills = ".NET,SQL";

            // Act
            double result = SkillHelper.CalculateSkillMatchPercentage(
                employeeSkills,
                requiredSkills);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CalculateSkillMatchPercentage_ShouldIgnoreCase()
        {
            // Arrange
            string employeeSkills = ".net,angular,sql";
            string requiredSkills = ".NET,SQL";

            // Act
            double result = SkillHelper.CalculateSkillMatchPercentage(
                employeeSkills,
                requiredSkills);

            // Assert
            result.Should().Be(100);
        }

        [Fact]
        public void CalculateSkillMatchPercentage_ShouldReturn0_WhenRequiredSkillsEmpty()
        {
            // Arrange
            string employeeSkills = ".NET,Angular";
            string requiredSkills = "";

            // Act
            double result = SkillHelper.CalculateSkillMatchPercentage(
                employeeSkills,
                requiredSkills);

            // Assert
            result.Should().Be(0);
        }
    }
}