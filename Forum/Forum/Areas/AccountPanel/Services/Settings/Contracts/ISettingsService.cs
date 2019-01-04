using Forum.Models;
using Forum.Web.Areas.Settings.ViewModels.Settings;
using System.Security.Claims;

namespace Forum.Web.Areas.AccountPanel.Services.Settings.Contracts
{
    public interface ISettingsService
    {
        void ChangePassword(ForumUser user, string oldPassword, string newPassword);

        bool CheckPassword(ForumUser user, string password);

        bool DeleteAccount(ForumUser user);

        EditProfileInputModel MapEditModel(string username);

        bool EditProfile(ForumUser user, EditProfileInputModel model);

        byte[] BuildFile(ClaimsPrincipal principal);
    }
}