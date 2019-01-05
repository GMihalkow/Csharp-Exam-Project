using Forum.Models;
using Forum.ViewModels.Interfaces.Account;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Services.Interfaces.Account
{
    public interface IAccountService
    {
        IEnumerable<string> GetUsernamesWithoutOwner();

        ForumUser GetUserByName(string username);

        ForumUser GetUserById(string id);

        bool UsernameExists(string username);

        ForumUser GetUser(ClaimsPrincipal principal);

        void LoginUser(ILoginUserInputModel model);

        void RegisterUser(IRegisterUserViewModel model);

        void LogoutUser();

        string GetNewestUser();

        Task<bool> EmailExists(string email);

        bool UserExists(string username);

        Task<bool> UserWithPasswordExists(string username, string password);

        IEnumerable<ForumUser> GetUsers();

        IEnumerable<string> GetUsernames();
    }
}