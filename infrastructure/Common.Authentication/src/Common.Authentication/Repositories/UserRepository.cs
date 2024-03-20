// using Common.Authentication.Models;

// namespace Common.Authentication.Repositories;

// public class UserRepository
// {
//     private readonly List<UserAccount> _users;

//     public UserRepository()
//     {
//         _users = new()
//         {
//             new UserAccount {
//                 UserId = "3AB9C947-30A5-47A7-A444-3AFD1F6CAE30",
//                 UserName = "admin",
//                 Email = "admin@my-company.com",
//                 Password = "admin",
//                 RoleName = "Administrator",
//                 RoleId = "501B6851-6EAE-4643-AB81-2E8993A2E81F"},
//             new UserAccount {
//                 UserId = "2F1D6B4F-8460-4D12-A474-79576DCB68F9",
//                 UserName = "user",
//                 Email = "user@my-company.com",
//                 Password = "user",
//                 RoleName = "User",
//                 RoleId = "1377BBC3-59A5-4D68-AE15-D629CEC2796D"
//                 }
//         };
//     }

//     public async Task<UserAccount?> GetUserAsync(string identifier, string password)
//     {

//         return await Task.Run(() =>
//         {
//             UserAccount? user = null;
//             // Check if identifier is an email
//             if (identifier.Contains("@"))
//             {
//                 user = _users.FirstOrDefault(user => user.Email == identifier && user.Password == password);
//             }
//             else if (identifier != null)
//             {
//                 user = _users.FirstOrDefault(user => user.UserName == identifier && user.Password == password);

//             }
//             else if (identifier is null)
//             {
//                 throw new InvalidOperationException("User could not be found.");
//             }

//             return user;
//         });
//     }
// }
