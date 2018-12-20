using Forum.Web.Services.Account.Contracts;
using Forum.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers.Account
{
    public class AccountController : BaseController
    {
        public AccountController(IAccountService accountService) : base(accountService)
        {
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserInputModel model)
        {
            if (ModelState.IsValid)
            {
                this.accountService.LoginUser(model);
                return this.Redirect("/");
            }
            else
            {
                return this.View(model);
            }
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.accountService.RegisterUser(model);

                return this.Redirect("/");
            }
            else
            {
                return this.View(model);
            }
        }

        [Authorize]
        public IActionResult Profile()
        {
            //TODO: Finish implementing profile view
            return this.View();
        }

        public IActionResult Dismiss()
        {
            this.accountService.LogoutUser();

            return this.Redirect("/Account/Logout");
        }

        public IActionResult Logout()
        {
            return this.View();
        }

        [Authorize]
        public PartialViewResult MyProfile()
        {
            return this.PartialView("_MyProfilePartial");
        }

        [Authorize]
        public PartialViewResult Settings()
        {
            return this.PartialView("_SettingsPartial");
        }

        [Authorize]
        public PartialViewResult ChangeUsername()
        {
            return this.PartialView("_ChangeUsernamePartial");
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangeUsername(string oldUsername, string newUsername)
        {
            var user = this.accountService.GetUserByName(oldUsername);
            if(user == null || user.UserName != this.User.Identity.Name)
            {
                return this.NotFound();
            }

            this.accountService.ChangeUsername(user, newUsername);

            return this.View("Profile");
        }
    }
}