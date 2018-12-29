﻿namespace Forum.Web.Services.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using CloudinaryDotNet;
    using Forum.Services.Interfaces.Db;
    using Forum.Web.Services.Account.Contracts;
    using Forum.Web.Utilities;
    using Forum.Web.ViewModels.Account;
    using global::Forum.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IOptions<CloudConfiguration> CloudConfig;
        private readonly UserManager<ForumUser> userManager;
        private readonly SignInManager<ForumUser> signInManager;
        private readonly IDbService dbService;

        public AccountService(IMapper mapper, IOptions<CloudConfiguration> CloudConfig, UserManager<ForumUser> userManager, SignInManager<ForumUser> signInManager, IDbService dbService)
        {
            this.mapper = mapper;
            this.CloudConfig = CloudConfig;
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
            this.OnPostRegisterAsync(model, model.Password);
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

        public IdentityResult OnPostRegisterAsync(RegisterUserViewModel viewModel, string password)
        {
            var model =
                 this.mapper
                 .Map<RegisterUserViewModel, ForumUser>(viewModel);

            CloudinaryDotNet.Account cloudAccount = new CloudinaryDotNet.Account(this.CloudConfig.Value.CloudName, this.CloudConfig.Value.ApiKey, this.CloudConfig.Value.ApiSecret);

            Cloudinary cloudinary = new Cloudinary(cloudAccount);

            var stream = viewModel.Image.OpenReadStream();

            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new FileDescription(viewModel.Image.FileName, stream),
                PublicId = $"{model.UserName}_profile_pic"
            };
            
            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

            string url = cloudinary.Api.UrlImgUp.BuildUrl($"{model.UserName}_profile_pic");

            model.ProfilePicutre = url;

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

        public bool DeleteAccount(ForumUser user)
        {
            var result = this.userManager.DeleteAsync(user).GetAwaiter().GetResult();

            return result.Succeeded;
        }

        public EditProfileInputModel MapEditModel(string username)
        {
            var user = this.GetUserByName(username);

            var model = this.mapper.Map<EditProfileInputModel>(user);

            return model;
        }

        public bool ChangeLocation(ForumUser user, string newLocation)
        {
            user.Location = newLocation;
            var result = this.dbService.DbContext.SaveChanges();

            if(result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ChangeGender(ForumUser user, string newGender)
        {
            user.Gender = newGender;
            var result = this.dbService.DbContext.SaveChanges();

            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ProfileInfoViewModel GetProfileInfo(ClaimsPrincipal principal)
        {
            var user =
                this.dbService
                .DbContext
                .Users
                .Include(u => u.Posts)
                .Where(u => u.UserName == principal.Identity.Name)
                .FirstOrDefault();

            var model = this.mapper.Map<ProfileInfoViewModel>(user);

            return model;
        }

        public IEnumerable<string> GetUsernames()
        {
            var usernames =
                this.dbService
                .DbContext
                .Users
                .Select(u => u.UserName)
                .ToList();

            return usernames;
        }
    }
}