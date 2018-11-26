namespace Forum.Web.Services.Contracts
{
    using Forum.Models;
    using Forum.Web.ViewModels.Account;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IAccountService
    {
        int GetUsersCount();

        ForumUser GetUser(ClaimsPrincipal principal);

        IActionResult LoginUser(LoginUserInputModel model);

        IActionResult LogoutUser();

        string GetNewestUser();

        Task<bool> EmailExists(string email);

        bool UserExists(string username);

        Task<bool> UserWithPasswordExists(string username, string password);

        IActionResult RegisterUser(RegisterUserViewModel model);
    }
}