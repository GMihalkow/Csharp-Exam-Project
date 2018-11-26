namespace Forum.Web.Controllers.Account
{
    using global::Forum.Web.Services.Contracts;
    using global::Forum.Web.ViewModels.Account;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
                var result = this.accountService.LoginUser(model);
                return result;
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
                var result = this.accountService.RegisterUser(model);

                return result;
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
            var result = this.accountService.LogoutUser();

            return this.Redirect("/Account/Logout");
        }

        public IActionResult Logout()
        {
            return this.View();
        }
    }
}