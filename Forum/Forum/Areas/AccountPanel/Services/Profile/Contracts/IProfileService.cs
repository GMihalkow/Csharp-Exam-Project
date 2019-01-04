using Forum.Web.Areas.AccountPanel.ViewModels.Profile;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Web.Areas.Profile.Services.Account.Contracts
{
    public interface IProfileService
    {
        ProfileInfoViewModel GetProfileInfo(ClaimsPrincipal principal);

        bool IsImageExtensionValid(string fileName);

        void UploadProfilePicture(IFormFile image, string username);
    }
}