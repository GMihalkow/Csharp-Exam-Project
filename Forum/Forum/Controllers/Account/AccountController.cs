namespace Forum.Web.Controllers.Account
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Account.Contracts;
    using global::Forum.Web.ViewModels.Account;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class AccountController : BaseController
    {
        private readonly IMapper mapper;

        public AccountController(IMapper mapper,IAccountService accountService) : base(accountService)
        {
            this.mapper = mapper;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserInputModel model)
        {
            ForumUser user =
                this.mapper
                .Map<ForumUser>(model);

            if (ModelState.IsValid)
            {
                this.accountService.LoginUser(user, model.Password);
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
            var user =
                this.mapper
                .Map<ForumUser>(model);

            if (ModelState.IsValid)
            {
                this.accountService.RegisterUser(user, model.Password);

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
    }
}