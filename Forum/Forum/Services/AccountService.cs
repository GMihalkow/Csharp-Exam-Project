namespace Forum.Web.Services
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Forum.Models;
    using Forum.Web.Services.Contracts;
    using Forum.Web.ViewModels.Account;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AccountService : IAccountService
    {
        private readonly UserManager<ForumUser> userManager;
        private readonly SignInManager<ForumUser> signInManager;
        private readonly DbService dbService;

        public AccountService(UserManager<ForumUser> userManager, SignInManager<ForumUser> signInManager, DbService dbService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbService = dbService;
        }

        public IActionResult LoginUser(LoginUserInputModel model)
        {
            return this.OnPostLoginAsync(model).Result;
        }

        public IActionResult RegisterUser(RegisterUserViewModel model)
        {
            return this.OnPostRegisterAsync(model).Result;
        }

        public IActionResult LogoutUser()
        {
            return this.OnGetLogout().Result;
        }

        public async Task<IActionResult> OnGetLogout()
        {
            await signInManager.SignOutAsync();

            var result = new RedirectResult("/");

            return result;
        }

        public async Task<bool> EmailExists(string email)
        {
            ForumUser user = await this.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<IActionResult> OnPostRegisterAsync(RegisterUserViewModel model)
        {
            var user = new ForumUser { UserName = model.Username, Gender = model.Gender, Location = model.Country, Email = model.Email, RegisteredOn = DateTime.UtcNow, LastActiveOn = DateTime.UtcNow };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (this.dbService.DbContext.Users.Count() == 1)
                {
                    await this.userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await this.userManager.AddToRoleAsync(user, "User");
                }

                await signInManager.SignInAsync(user, isPersistent: false);
            }

            var actionResult = new RedirectResult("/");

            return actionResult;
        }

        public async Task<IActionResult> OnPostLoginAsync(LoginUserInputModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: true);

            var actionResult = new RedirectResult("/");

            return actionResult;
        }

        public bool UserExists(string username)
        {
            var result = this.dbService.DbContext.Users.Any(u => u.UserName == username);

            return result;
        }

        public async Task<bool> UserWithPasswordExists(string username, string password)
        {
            if (!this.UserExists(username))
            {
                return false;
            }

            var User = this.dbService.DbContext.Users.First(u => u.UserName == username);

            var result = await this.signInManager.PasswordSignInAsync(User, password, false, false);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public ForumUser GetUser(ClaimsPrincipal principal)
        {
            ForumUser user = this.userManager.GetUserAsync(principal).GetAwaiter().GetResult();

            return user;
        }

        public int GetUsersCount()
        {
            int usersCount = this.dbService.DbContext.Users.Count();
            return usersCount;
        }

        public string GetNewestUser()
        {
            ForumUser user =
                this.dbService
                .DbContext
                .Users
                .OrderByDescending(u => u.RegisteredOn)
                .FirstOrDefault();

            string username = string.Empty;
            if(user != null)
            { 
                username = user.UserName;
            }

            return username;
        }
    }
}