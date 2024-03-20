// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.Extensions.Logging;
// using Microsoft.IdentityModel.Tokens;
// using Common.Authentication.Models;
// using Common.Authentication.Repositories;
// using System.Security.Cryptography;

// namespace Common.Authentication.JwtAuthentication;

// public class JwtTokenHandler
// {

//     private readonly UserRepository _userRepository;
//     private readonly ILogger<JwtTokenHandler> _logger;
//     private const int JWT_TOKEN_EXPIRY_IN_MINUTES = 20;

//     public JwtTokenHandler(
//         UserRepository repo,
//         ILogger<JwtTokenHandler> logger
//     )
//     {
//         _userRepository = new UserRepository();
//         _logger = logger;
//     }



//     public string GenerateSecureToken(int length)
//     {
//         var byteArr = new byte[length];
//         RandomNumberGenerator.Fill(byteArr);

//         return Convert.ToBase64String(byteArr);

//     }


// }
