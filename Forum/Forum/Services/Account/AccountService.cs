namespace Forum.Web.Services.Account
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using Forum.Services.Interfaces.Db;
    using Forum.Web.Services.Account.Contracts;
    using Forum.Web.ViewModels.Account;
    using global::Forum.Models;
    using Microsoft.AspNetCore.Identity;

    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly UserManager<ForumUser> userManager;
        private readonly SignInManager<ForumUser> signInManager;
        private readonly IDbService dbService;

        public AccountService(IMapper mapper, UserManager<ForumUser> userManager, SignInManager<ForumUser> signInManager, IDbService dbService)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbService = dbService;
        }

        public void LoginUser(LoginUserInputModel model)
        {
            var user =
                this.mapper
                .Map<LoginUserInputModel, ForumUser>(model);

            this.OnPostLoginAsync(user, model.Password).GetAwaiter().GetResult();
        }

        public void RegisterUser(RegisterUserViewModel model)
        {
            var user =
                 this.mapper
                 .Map<RegisterUserViewModel, ForumUser>(model);

            this.OnPostRegisterAsync(user, model.Password);
        }

        public void LogoutUser()
        {
            this.OnGetLogout().GetAwaiter().GetResult();
        }

        public async Task OnGetLogout()
        {
            await signInManager.SignOutAsync();
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

        public IdentityResult OnPostRegisterAsync(ForumUser model, string password)
        {
            model.RegisteredOn = DateTime.UtcNow;
            var result = userManager.CreateAsync(model, password).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                if (this.dbService.DbContext.Users.Count() == 1)
                {
                    this.userManager.AddToRoleAsync(model, "Administrator").GetAwaiter().GetResult();
                }
                else
                {
                    this.userManager.AddToRoleAsync(model, "User").GetAwaiter().GetResult();
                }

                signInManager.SignInAsync(model, isPersistent: false).GetAwaiter().GetResult();
            }

            return result;
        }

        public async Task<SignInResult> OnPostLoginAsync(ForumUser model, string password)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await this.signInManager.PasswordSignInAsync(model.UserName, password, false, lockoutOnFailure: true);

            return result;
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
            if (user != null)
            {
                username = user.UserName;
            }

            return username;
        }

        public int GetTotalPostsCount()
        {
            int totalPostsCount =
                this.dbService
                .DbContext
                .Posts.Count();

            return totalPostsCount;
        }

        public bool UsernameExists(string username)
        {
            var result =
                this.dbService
                .DbContext
                .Users
                .Any(u => u.UserName == username);

            return result;
        }

        public ForumUser GetUserById(string id)
        {
            var user =
                this.dbService
                .DbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            return user;
        }

        public bool ChangeUsername(ForumUser user, string username)
        {
            var result = this.userManager.SetUserNameAsync(user, username).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ForumUser GetUserByName(string username)
        {
            var user =
                this.dbService
                .DbContext
                .Users
                .Where(u => u.UserName == username)
                .FirstOrDefault();

            return user;
        }

        public bool ChangePassword(ForumUser user, string oldPassword, string newPassword)
        {
            var result = this.userManager.ChangePasswordAsync(user, oldPassword, newPassword).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckPassword(ForumUser user, string password)
        {
            var result = this.userManager.CheckPasswordAsync(user, password).GetAwaiter().GetResult();

            return result;
        }
    }
}