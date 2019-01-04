namespace Forum.Web.Services.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using Forum.Services.Interfaces.Db;
    using Forum.Web.Areas.Profile.Services.Account.Contracts;
    using Forum.Web.Services.Account.Contracts;
    using Forum.Web.Utilities;
    using Forum.Web.ViewModels.Account;
    using global::Forum.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly UserManager<ForumUser> userManager;
        private readonly IOptions<CloudConfiguration> cloudConfig;
        private readonly SignInManager<ForumUser> signInManager;
        private readonly IDbService dbService;
        private readonly IProfileService profileService;

        public AccountService(IMapper mapper, UserManager<ForumUser> userManager, IOptions<CloudConfiguration> CloudConfig, SignInManager<ForumUser> signInManager, IDbService dbService, IProfileService profileService)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            cloudConfig = CloudConfig;
            this.signInManager = signInManager;
            this.dbService = dbService;
            this.profileService = profileService;
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
            this.OnPostRegisterAsync(model, model.Password);
        }

        public void LogoutUser()
        {
            this.signInManager.SignOutAsync().GetAwaiter().GetResult();
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

        public IdentityResult OnPostRegisterAsync(RegisterUserViewModel viewModel, string password)
        {
            var model =
                 this.mapper
                 .Map<RegisterUserViewModel, ForumUser>(viewModel);

            model.RegisteredOn = DateTime.UtcNow;

            var result = userManager.CreateAsync(model, password).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                if (this.dbService.DbContext.Users.Count() == 1)
                {
                    this.userManager.AddToRoleAsync(model, "Owner").GetAwaiter().GetResult();
                }
                else if (this.dbService.DbContext.Users.Count() == 2)
                {
                    this.userManager.AddToRoleAsync(model, "Administrator").GetAwaiter().GetResult();
                }
                else
                {
                    this.userManager.AddToRoleAsync(model, "User").GetAwaiter().GetResult();
                }

                if (viewModel.Image != null)
                {
                    this.profileService.UploadProfilePicture(viewModel.Image, viewModel.Username);
                }

                signInManager.SignInAsync(model, isPersistent: false).GetAwaiter().GetResult();
            }


            return result;
        }

        public async Task<SignInResult> OnPostLoginAsync(ForumUser model, string password)
        {
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

            return result.Succeeded;
        }

        public ForumUser GetUser(ClaimsPrincipal principal)
        {
            ForumUser user = this.userManager.GetUserAsync(principal).GetAwaiter().GetResult();

            return user;
        }

        public string GetNewestUser()
        {
            ForumUser user = this.dbService.DbContext.Users.OrderByDescending(u => u.RegisteredOn).FirstOrDefault();

            string username = string.Empty;
            if (user != null)
            {
                username = user.UserName;
            }

            return username;
        }

        public bool UsernameExists(string username)
        {
            var result = this.dbService.DbContext.Users.Any(u => u.UserName == username);

            return result;
        }

        public ForumUser GetUserById(string id)
        {
            var user = this.dbService.DbContext.Users.FirstOrDefault(u => u.Id == id);

            return user;
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

        public IEnumerable<string> GetUsernames()
        {
            var usernames = this.GetUsers().Select(u => u.UserName).ToList();

            return usernames;
        }

        public IEnumerable<ForumUser> GetUsers()
        {
            var users = this.dbService.DbContext.Users.ToList();

            return users;
        }

        public IEnumerable<string> GetUsernamesWithoutOwner()
        {
            var usernames = this.GetUsers()
                .Where(u => !this.userManager.IsInRoleAsync(u, Common.Role.Owner).GetAwaiter().GetResult())
                .Select(u => u.UserName)
                .ToList();

            return usernames;
        }

        public int GetPagesCount(int usersCount)
        {
            var result = (int)Math.Ceiling(usersCount / 5.0);

            return result;
        }
    }
}