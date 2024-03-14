using Common.Authentication.Models;

namespace Common.Authentication.Repositories;

public class UserRepository
{
    private readonly List<UserAccount> _users;

    public UserRepository()
    {
        _users = new()
        {
            new UserAccount { UserName = "admin", Password = "admin", Role = "Administrator" },
            new UserAccount { UserName = "user", Password = "user", Role = "User" }
        };
    }

    public async Task<UserAccount?> GetUserAsync(string userName, string password)
    {
        return await Task.Run(() =>
        {
            var user = _users.FirstOrDefault(x => x.UserName == userName && x.Password == password);

            if (user is null)
            {
                throw new InvalidOperationException("User could not be found.");
            }

            return user;
        });
    }
}
