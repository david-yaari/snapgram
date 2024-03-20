using Common.Authentication.Models;
using Common.Authentication.JwtAuthentication;
using Xunit.Abstractions;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace Common.JwtAuthentication.Tests;

public class JwtTokenGeneratorTests
{
    [Fact]
    public async Task GenerateJwtToken_ShouldReturnValidToken()
    {
        // Arrange
        var jwtTokenRequest = new JwtTokenRequest
        {
            UserId = Guid.NewGuid(),
            RolesId = new List<Guid> { Guid.NewGuid() }
        };

        var loggerMock = new Mock<ILogger<JwtTokenGenerator>>();

        var jwtTokenGenerator = new JwtTokenGenerator(loggerMock.Object);

        // Act
        var result = await jwtTokenGenerator.GenerateJwtToken(jwtTokenRequest);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.JwtToken);
        // Add more assertions here based on what you expect the result to be...
    }

    [Fact]
    public async Task GenerateJwtToken_ShouldFail_When_JwtTokenRequestIsEmpty()
    {
        // Arrange
        var jwtTokenRequest = new JwtTokenRequest(); // Empty JwtTokenRequest

        var loggerMock = new Mock<ILogger<JwtTokenGenerator>>();
        var jwtTokenGenerator = new JwtTokenGenerator(loggerMock.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => jwtTokenGenerator.GenerateJwtToken(jwtTokenRequest));
    }

    [Fact]
    public async Task ValidateJwtToken_ShouldValidateToken()
    {
        // Arrange
        var jwtTokenRequest = new JwtTokenRequest
        {
            UserId = Guid.NewGuid(),
            RolesId = new List<Guid> { Guid.NewGuid() }
        };

        var loggerMock = new Mock<ILogger<JwtTokenGenerator>>();

        var jwtTokenGenerator = new JwtTokenGenerator(loggerMock.Object);

        // Act
        var token = await jwtTokenGenerator.GenerateJwtToken(jwtTokenRequest);

        var jwtTokenValidator = new JwtTokenValidator();

        var result = jwtTokenValidator.ValidateJwtToken(token!.JwtToken!);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Claims);
        Assert.True(result.Identity!.IsAuthenticated);
        // Add more assertions here based on what you expect the result to be...
    }

    [Fact]
    public async Task ValidateJwtToken_ShouldFail_When_TokenIsInvalid()
    {
        // Arrange
        var jwtTokenRequest = new JwtTokenRequest
        {
            UserId = Guid.NewGuid(),
            RolesId = new List<Guid> { Guid.NewGuid() }
        };

        var loggerMock = new Mock<ILogger<JwtTokenGenerator>>();

        var jwtTokenGenerator = new JwtTokenGenerator(loggerMock.Object);

        // Act
        var token = await jwtTokenGenerator.GenerateJwtToken(jwtTokenRequest);
        var jwtToken = token!.JwtToken + " "; // Add a space to the token to make it invalid

        var jwtTokenValidator = new JwtTokenValidator();

        // Assert
        Assert.Throws<SecurityTokenException>(() => jwtTokenValidator.ValidateJwtToken(jwtToken));

        // Add more assertions here based on what you expect the result to be...
    }

    [Fact]
    public Task ValidateJwtToken_ShouldFail_When_TokenIsTampered()
    {
        // Arrange
        var jwtTokenRequest = new JwtTokenRequest
        {
            UserId = Guid.NewGuid(),
            RolesId = new List<Guid> { Guid.NewGuid() }
        };

        var loggerMock = new Mock<ILogger<JwtTokenGenerator>>();

        var jwtTokenGenerator = new JwtTokenGenerator(loggerMock.Object);

        // Act
        var jwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE3MTA5MzQ1MDcsImV4cCI6MTc0MjQ3MDY1NSwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2tldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2NrZXQiLCJFbWFpbCI6Impyb2NrZXRAZXhhbXBsZS5jb20iLCJSb2xlIjpbIk1hbmFnZXIiLCJQcm9qZWN0IEFkbWluaXN0cmF0b3IiXX0.8l34z9QnpgHVx0ZzmkI6DbRS5RkZmmJw_EQkR3R4_AA"; // Tampered token

        var jwtTokenValidator = new JwtTokenValidator();

        // Assert
        Assert.Throws<SecurityTokenException>(() => jwtTokenValidator.ValidateJwtToken(jwtToken));

        return Task.CompletedTask;
        // Add more assertions here based on what you expect the result to be...
    }
}

// public class JwtTokenGeneratorTests
// {
//     private readonly ITestOutputHelper output;

//     public JwtTokenGeneratorTests(ITestOutputHelper output)
//     {
//         this.output = output;

//         // Create a mock ILogger
//         var mockLogger = new Mock<ILogger<JwtTokenGenerator>>();

//         // Initialize JwtTokenHandler with the mock dependencies
//         _jwtTokenHandler = new JwtTokenHandler(mockRepo.Object, mockLogger.Object);

//     }

//     [Theory]
//     [InlineData("admin@my-company.com", "admin")]
//     [InlineData("user@my-company.com", "user")]
//     public async Task JwtTokenHandler_Tests_Test_JwtTokenForValidUser_ShouldWork(string email, string password)
//     {
//         // Arrange
//         // Create an instance of UserRepository
//         UserRepository repo = new UserRepository();

//         // Create a mock ILogger
//         var mockLogger = new Mock<ILogger<JwtTokenHandler>>();


//         // Create an instance of JwtTokenHandler
//         JwtTokenHandler tokenHandler = new JwtTokenHandler(repo, mockLogger.Object);

//         // Act
//         AuthenticationResponse? response = await tokenHandler.GenerateJwtToken(new AuthenticationRequest { Email = email, Password = password });
//         // Assert
//         Assert.NotNull(response);
//         Assert.NotNull(response.UserId);
//         Assert.NotNull(response.JwtToken);
//         Assert.True(response.JwtTokenExpiryTime > 0);
//     }

//     [Theory]
//     [InlineData("", "admin")]
//     [InlineData("admin@my-company.com", "")]
//     [InlineData("", "")]
//     public async Task JwtTokenHandler_Tests_Test_JwtTokenForInvalidUser_ShouldThrowException(string email, string password)
//     {
//         // Arrange
//         // Create an instance of UserRepository
//         UserRepository repo = new UserRepository();

//         // Create a mock ILogger
//         var mockLogger = new Mock<ILogger<JwtTokenHandler>>();

//         // Create an instance of JwtTokenHandler
//         JwtTokenHandler tokenHandler = new JwtTokenHandler(repo, mockLogger.Object);

//         // Act

//         // Assert
//         await Assert.ThrowsAsync<ArgumentException>(() => tokenHandler.GenerateJwtToken(new AuthenticationRequest { Email = email, Password = password }));
//     }

//     [Theory]
//     [InlineData("admin@my-company.com", "admin")]
//     [InlineData("user@my-company.com", "user")]
//     public async Task ValidateToken_ValidToken_ReturnsCorrectClaimsPrincipal(string email, string password)
//     {
//         // Arrange
//         var userId = Guid.NewGuid().ToString();
//         var authenticationResponse = await _jwtTokenHandler.GenerateJwtToken(new AuthenticationRequest { Email = email, Password = password });
//         var jwtToken = authenticationResponse!.JwtToken;

//         // Act
//         var claimsPrincipal = _jwtTokenHandler.ValidateToken(jwtToken!);

//         // Assert
//         Assert.NotNull(claimsPrincipal);
//         var claim = claimsPrincipal.Claims.First(c => c.Type == "id");
//         Assert.Equal(userId, claim.Value);
//     }

// }