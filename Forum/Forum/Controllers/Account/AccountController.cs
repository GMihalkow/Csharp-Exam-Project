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
            var model = this.accountService.GetProfileInfo(this.User);

            return this.PartialView("_MyProfilePartial", model);
        }

        [Authorize]
        public PartialViewResult Settings()
        {
            return this.PartialView("_SettingsPartial");
        }

        [Authorize]
        public PartialViewResult EditProfile()
        {
            var model = this.accountService.MapEditModel(this.User.Identity.Name);

            return this.PartialView("_EditProfilePartial", model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditProfile(EditProfileInputModel model)
        {
            var user = this.accountService.GetUserByName(this.User.Identity.Name);

            var isUsernameChanged = this.accountService.ChangeUsername(user, model.Username);
          
            var passwordCheck = this.accountService.CheckPassword(user, model.Password);
            if (!passwordCheck)
            {
                return this.BadRequest();
            }

            var isLocationChanged = this.accountService.ChangeLocation(user, model.Location);

            var isGenderChanged = this.accountService.ChangeGender(user, model.Gender);

            return this.View("Profile");
        }

        [Authorize]
        public PartialViewResult ChangePassword()
        {
            return this.PartialView("_ChangePasswordPartial");
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var user = this.accountService.GetUser(this.User);

            var passwordCheck = this.accountService.CheckPassword(user, oldPassword);
            if (!passwordCheck)
            {
                return this.BadRequest();
            }

            var result = this.accountService.ChangePassword(user, oldPassword, newPassword);
            if (!result)
            {
                return this.BadRequest();
            }

            return this.View("Profile");
        }

        [Authorize]
        public PartialViewResult DeleteAccount()
        {
            return this.PartialView("_DeleteAccountPartial");
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteAccount(string username, string password)
        {
            var user = this.accountService.GetUserByName(username);
            if (user == null || user.UserName != username)
            {
                return this.BadRequest();
            }

            var passwordCheck = this.accountService.CheckPassword(user, password);
            if (!passwordCheck)
            {
                return this.BadRequest();
            }

            this.accountService.LogoutUser();

            this.accountService.DeleteAccount(user);

            return this.Redirect("/");
        }

        [Authorize]
        public PartialViewResult MessagesPanel()
        {
            return this.PartialView("_MessagesPanelPartial");
        }
    }
}