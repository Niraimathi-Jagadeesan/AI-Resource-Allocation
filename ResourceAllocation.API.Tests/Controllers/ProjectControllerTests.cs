using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAllocation.API.Controllers;
using ResourceAllocation.API.Data;
using ResourceAllocation.API.Models;
using Xunit;

namespace ResourceAllocation.API.Tests.Controllers
{
    public class ProjectControllerTests
    {
        private readonly AppDbContext _context;
        private readonly ProjectsController _controller;

        public ProjectControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _context.Projects.Add(new Project
            {
                Id = 1,
                ProjectName = "AI Platform",
                RequiredSkills = ".NET,Angular",
                MinimumExperience = 5,
                Complexity = "High",
                Priority = 8
            });

            _context.SaveChanges();

            _controller = new ProjectsController(_context);
        }

        [Fact]
        public async Task GetAll_ShouldReturnProjects()
        {
            var result = await _controller.GetAll();

            var projects =
                Assert.IsAssignableFrom<IEnumerable<Project>>(
                    result.Value);

            Assert.Single(projects);
        }

        [Fact]
        public async Task Post_ShouldCreateProject()
        {
            var project = new Project
            {
                ProjectName = "Automation",
                RequiredSkills = "Playwright",
                MinimumExperience = 3,
                Complexity = "Medium",
                Priority = 5
            };

            var result = await _controller.Create(project);

            var createdResult =
                Assert.IsType<CreatedAtActionResult>(
                    result.Result);

            Assert.Equal(2, _context.Projects.Count());
        }

        [Fact]
        public async Task Delete_ShouldRemoveProject()
        {
            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(_context.Projects);
        }
    }
}