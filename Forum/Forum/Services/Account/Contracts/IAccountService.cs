using Forum.Models;
using Forum.Web.ViewModels.Account;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Web.Services.Account.Contracts
{

    public interface IAccountService
    {
        int GetUsersCount();

        bool UsernameExists(string username);

        int GetTotalPostsCount();

        ForumUser GetUser(ClaimsPrincipal principal);

        void LoginUser(LoginUserInputModel model);

        void RegisterUser(RegisterUserViewModel model);

        void LogoutUser();

        string GetNewestUser();

        Task<bool> EmailExists(string email);

        bool UserExists(string username);

        Task<bool> UserWithPasswordExists(string username, string password);
    }
}