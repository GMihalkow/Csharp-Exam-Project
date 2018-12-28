using Forum.Models;
using Forum.Web.ViewModels.Account;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Web.Services.Account.Contracts
{

    public interface IAccountService
    {
        ForumUser GetUserByName(string username);

        int GetUsersCount();

        ForumUser GetUserById(string id);

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

        bool ChangeUsername(ForumUser user, string username);

        bool ChangePassword(ForumUser user, string oldPassword, string newPassword);

        bool CheckPassword(ForumUser user, string password);

        bool DeleteAccount(ForumUser user);

        EditProfileInputModel MapEditModel(string username);

        bool ChangeLocation(ForumUser user, string newLocation);

        bool ChangeGender(ForumUser user, string newGender);

        ProfileInfoViewModel GetProfileInfo(ClaimsPrincipal principal);

        IEnumerable<string> GetUsernames();
    }
}