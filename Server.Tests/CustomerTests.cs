using System.Runtime.InteropServices;
using BLL.Services;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.SQL.Database.Model;
using Moq;

namespace Server.Tests;

public class CustomerTests
{
    private readonly Mock<ICustomerRepo> _mockRepo;
    private readonly CustomerService _customerService;

    public CustomerTests()
    {
        _mockRepo = new Mock<ICustomerRepo>();
        _customerService = new CustomerService(_mockRepo.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("; DROP TABLE Users;")]
    [InlineData("mojID")]
    [InlineData("ef657dd4-58f6-43a5-89f7-dd84c8b13d9")]
    public async Task GetUser_ShouldThrowArgumentException(string invalidUserId)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _customerService.RetrieveAsync(invalidUserId));
    }

    [Fact]
    public async Task GetUser_ShouldReturnUser()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var customer = new ApiUser{
            FirstName = "Pera",
            LastName = "Peric",
            Email = "pera@gmail.com",
            UserName = "pera"
        };
        _mockRepo.Setup(r => r.RetrieveAsync(userId)).ReturnsAsync(customer);

        // Act
        var result = await _customerService.RetrieveAsync(userId);

        // Assert
        Assert.NotNull(result);
        _mockRepo.Verify(r => r.RetrieveAsync(userId), Times.Once);
    }
}