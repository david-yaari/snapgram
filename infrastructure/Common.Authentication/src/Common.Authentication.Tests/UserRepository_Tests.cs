// using Common.Authentication.Models;
// using Xunit.Abstractions;

// namespace Play.JwtAuthenticationManager.Tests;

// public class UserRepository_Tests
// {
//     private readonly ITestOutputHelper output;

//     public UserRepository_Tests(ITestOutputHelper output)
//     {
//         this.output = output;
//     }

//     [Theory]
//     [InlineData("admin", "admin")]
//     [InlineData("user", "user")]
//     public async Task UserRepository_Test_FindValidUser_ShouldWork(string userName, string password)
//     {
//         // Arrange
//         // Create an instance of UserRepository
//         UserRepository repo = new UserRepository();

//         // Act
//         UserAccount? userAccount = await repo.GetUserAsync(userName, password);
//         // Assert
//         Assert.NotNull(userAccount);
//         Assert.Equal(userName, userAccount.UserName);
//         Assert.Equal(password, userAccount.Password);
//     }

//     [Theory]
//     [InlineData("", "admin")]
//     [InlineData("adim", "")]
//     [InlineData("", "")]
//     [InlineData("admin", "addmin")]
//     [InlineData("user", "user1")]
//     public async Task UserRepository_Test_FindInvalidUser_ShouldThrowException(string userName, string password)
//     {
//         // Arrange
//         // Create an instance of UserRepository
//         UserRepository repo = new UserRepository();

//         // Act
//         // Assert
//         await Assert.ThrowsAsync<InvalidOperationException>(() => repo.GetUserAsync(userName, password));
//     }
// }