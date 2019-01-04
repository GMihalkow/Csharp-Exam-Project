using Forum.Models;
using Forum.Web.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Web.Services.Account.Contracts
{

    public interface IAccountService
    {
        IEnumerable<string> GetUsernamesWithoutOwner();

        ForumUser GetUserByName(string username);

        ForumUser GetUserById(string id);

        bool UsernameExists(string username);

        ForumUser GetUser(ClaimsPrincipal principal);

        void LoginUser(LoginUserInputModel model);

        void RegisterUser(RegisterUserViewModel model);

        void LogoutUser();

        string GetNewestUser();

        Task<bool> EmailExists(string email);

        bool UserExists(string username);

        Task<bool> UserWithPasswordExists(string username, string password);

        IEnumerable<ForumUser> GetUsers();

        IEnumerable<string> GetUsernames();

        int GetPagesCount(int usersCount);
    }
}