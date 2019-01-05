using AutoMapper;
using Forum.Models;
using Forum.Services.Interfaces.Account;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Message;
using Forum.Services.Interfaces.Quote;
using Forum.Services.Interfaces.Reply;
using Forum.Services.Interfaces.Report;
using Forum.Services.Interfaces.Settings;
using Forum.ViewModels.Interfaces.Settings;
using Forum.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace Forum.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;
        private readonly IReplyService replyService;
        private readonly SignInManager<ForumUser> signInManager;
        private readonly IQuoteService quoteService;
        private readonly IReportService reportService;
        private readonly IMessageService messageService;
        private readonly UserManager<ForumUser> userManager;
        private readonly IAccountService accountService;

        public SettingsService(IMapper mapper, IDbService dbService, IReplyService replyService, SignInManager<ForumUser> signInManager, IQuoteService quoteService, IReportService reportService, IMessageService messageService, UserManager<ForumUser> userManager, IAccountService accountService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
            this.replyService = replyService;
            this.signInManager = signInManager;
            this.quoteService = quoteService;
            this.reportService = reportService;
            this.messageService = messageService;
            this.userManager = userManager;
            this.accountService = accountService;
        }

        private int ChangeGender(ForumUser user, string newGender)
        {
            user.Gender = newGender;
            var result = this.dbService.DbContext.SaveChanges();

            return result;
        }

        private int ChangeLocation(ForumUser user, string newLocation)
        {
            user.Location = newLocation;
            var result = this.dbService.DbContext.SaveChanges();

            return result;
        }

        public void ChangePassword(ForumUser user, string oldPassword, string newPassword)
        {
            var result = this.userManager.ChangePasswordAsync(user, oldPassword, newPassword).GetAwaiter().GetResult();
        }

        private bool ChangeUsername(ForumUser user, string username)
        {
            var result = this.userManager.SetUserNameAsync(user, username).GetAwaiter().GetResult();

            return result.Succeeded;
        }

        public bool CheckPassword(ForumUser user, string password)
        {
            var result = this.userManager.CheckPasswordAsync(user, password).GetAwaiter().GetResult();

            return result;
        }

        public bool DeleteAccount(ForumUser user)
        {
            this.quoteService.DeleteUserQuotes(user);

            this.replyService.DeleteUserReplies(user.UserName);

            this.reportService.DeleteUserReports(user.UserName);

            this.messageService.RemoveUserMessages(user.UserName);

            var result = this.userManager.DeleteAsync(user).GetAwaiter().GetResult();

            return result.Succeeded;
        }

        public bool EditProfile(ForumUser user, IEditProfileInputModel model)
        {
            var isUsernameChanged = this.ChangeUsername(user, model.Username);

            var isLocationChanged = this.ChangeLocation(user, model.Location);

            var isGenderChanged = this.ChangeGender(user, model.Gender);

            return true;
        }

        public IEditProfileInputModel MapEditModel(string username)
        {
            var user = this.accountService.GetUserByName(username);

            var model = this.mapper.Map<EditProfileInputModel>(user);

            return model;
        }

        public byte[] BuildFile(ClaimsPrincipal principal)
        {
            var user = this.accountService.GetUserByName(principal.Identity.Name);

            var viewModel = this.mapper.Map<UserJsonViewModel>(user);

            var jsonStr = JsonConvert.SerializeObject(viewModel);

            var byteArr = Encoding.UTF8.GetBytes(jsonStr);

            return byteArr;
        }
    }
}