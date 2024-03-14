using Common.Authentication.Models;
using Common.Authentication.JwtAuthentication;
using Common.Authentication.Repositories;
using Xunit.Abstractions;
using Moq;
using Microsoft.Extensions.Logging;

namespace Play.JwtAuthentication.Tests;

public class JwtTokenHandler_Tests
{
    private readonly ITestOutputHelper output;

    public JwtTokenHandler_Tests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Theory]
    [InlineData("admin", "admin")]
    [InlineData("user", "user")]
    public async Task JwtTokenHandler_Tests_Test_JwtTokenForValidUser_ShouldWork(string userName, string password)
    {
        // Arrange
        // Create an instance of UserRepository
        UserRepository repo = new UserRepository();

        // Create a mock ILogger
        var mockLogger = new Mock<ILogger<JwtTokenHandler>>();


        // Create an instance of JwtTokenHandler
        JwtTokenHandler tokenHandler = new JwtTokenHandler(repo, mockLogger.Object);

        // Act
        AuthenticationResponse? response = await tokenHandler.GenerateJwtToken(new AuthenticationRequest { UserName = userName, Password = password });
        // Assert
        Assert.NotNull(response);
        Assert.Equal(userName, response.UserName);
        Assert.NotNull(response.JwtToken);
        Assert.True(response.JwtTokenExpiryTime > 0);
    }

    [Theory]
    [InlineData("", "admin")]
    [InlineData("adim", "")]
    [InlineData("", "")]
    public async Task JwtTokenHandler_Tests_Test_JwtTokenForInvalidUser_ShouldThrowException(string userName, string password)
    {
        // Arrange
        // Create an instance of UserRepository
        UserRepository repo = new UserRepository();

        // Create a mock ILogger
        var mockLogger = new Mock<ILogger<JwtTokenHandler>>();

        // Create an instance of JwtTokenHandler
        JwtTokenHandler tokenHandler = new JwtTokenHandler(repo, mockLogger.Object);

        // Act

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => tokenHandler.GenerateJwtToken(new AuthenticationRequest { UserName = userName, Password = password }));
    }
}