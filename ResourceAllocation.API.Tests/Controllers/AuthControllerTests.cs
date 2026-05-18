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
    public class AuthControllerTests
    {
        private readonly AppDbContext _context;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _context.Users.Add(new AppUser
            {
                Id = 1,
                Username = "admin",
                Password = "admin123",
                Role = "Admin"
            });

            _context.SaveChanges();

            var jwtServiceMock = new Mock<IJwtService>();
            jwtServiceMock
                .Setup(x => x.GenerateToken(It.IsAny<AppUser>()))
                .Returns("fake-jwt-token");

            _controller = new AuthController(
                _context,
                jwtServiceMock.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "admin123"
            };

            ActionResult<LoginResponse> actionResult = await _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var response = Assert.IsType<LoginResponse>(okResult.Value);

            // Assert
            Assert.Equal("fake-jwt-token", response.Token);
            Assert.Equal("admin", response.Username);
            Assert.Equal("Admin", response.Role);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsInvalid()
        {
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "wrong-password"
            };

            ActionResult<LoginResponse> actionResult = await _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
        }
    }
}