using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAllocation.API.Controllers;
using ResourceAllocation.API.Data;
using ResourceAllocation.API.Models;
using Xunit;

namespace ResourceAllocation.API.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly AppDbContext _context;
        private readonly EmployeesController _controller;

        public EmployeeControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _context.Employees.Add(new Employee
            {
                Id = 1,
                Name = "John",
                PrimaryRole = "Developer",
                Skills = ".NET,Angular",
                Experience = 5,
                PerformanceScore = 90,
                CurrentAllocationPercent = 20,
                IsAvailable = true
            });

            _context.SaveChanges();

            _controller = new EmployeesController(_context);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmployees()
        {
            var result = await _controller.GetAll();

            var employees = Assert.IsAssignableFrom<IEnumerable<Employee>>(result.Value);
            Assert.Single(employees);
        }

        [Fact]
        public async Task GetById_ShouldReturnEmployee()
        {
            var result = await _controller.GetById(1);

            var employee = Assert.IsType<Employee>(result.Value);
            Assert.Equal("John", employee.Name);
        }

        [Fact]
        public async Task Post_ShouldCreateEmployee()
        {
            var employee = new Employee
            {
                Name = "Jane",
                PrimaryRole = "Tester",
                Skills = "Playwright",
                Experience = 4,
                PerformanceScore = 85,
                CurrentAllocationPercent = 0,
                IsAvailable = true
            };

            var result = await _controller.Create(employee);

            var createdResult =
                Assert.IsType<CreatedAtActionResult>(result.Result);

            Assert.Equal(2, _context.Employees.Count());
        }

        [Fact]
        public async Task Delete_ShouldRemoveEmployee()
        {
            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(_context.Employees);
        }
    }
}