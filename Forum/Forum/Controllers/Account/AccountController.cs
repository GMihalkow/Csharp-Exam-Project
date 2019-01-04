using Forum.Services.Interfaces.Message;
using Forum.ViewModels.Message;
using Forum.Web.Attributes.CustomValidationAttributes;
using Forum.Web.Services.Account.Contracts;
using Forum.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Forum.Web.Controllers.Account
{
    public class AccountController : BaseController
    {
        private readonly IMessageService messageService;

        public AccountController(IAccountService accountService, IMessageService messageService) : base(accountService)
        {
            this.messageService = messageService;
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
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
            }
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model)
        {
            //TODO: Validate that files you upload are only images, (EndsWith .jpeg, png...)
            if (ModelState.IsValid)
            {
                this.accountService.RegisterUser(model);

                return this.Redirect("/");
            }
            else
            {
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
            }
        }

        [Authorize]
        public IActionResult Profile()
        {
            var user = this.accountService.GetUserByName(this.User.Identity.Name);

            this.ViewData["profilePicUrl"] = user.ProfilePicutre ?? null;

            return this.View();
        }

        [Authorize]
        public IActionResult Dismiss()
        {
            this.accountService.LogoutUser();

            return this.Redirect("/Account/Logout");
        }

        [Authorize]
        public IActionResult Logout()
        {
            return this.View();
        }
        

        [Authorize]
        public PartialViewResult MessagesPanel()
        {
            return this.PartialView("_MessagesPanelPartial");
        }

        [Authorize]
        public PartialViewResult Chat()
        {
            return this.PartialView("_WeclomeChatViewPartial");
        }

        [Authorize]
        public PartialViewResult RecentConversations()
        {
            this.ViewData["userNames"] = this.accountService.GetUsernames();

            this.ViewData["recentConversations"] = this.messageService.GetRecentConversations(this.User.Identity.Name);

            this.ViewData["unreadMessages"] = this.messageService.GetUnreadMessages(this.User.Identity.Name);

            return this.PartialView("_RecentConversationsPartial");
        }

        [Authorize]
        [HttpPost]
        public PartialViewResult ChatWithSomebody([FromBody] SendMessageInputModel model)
        {
            var recieverId = this.accountService.GetUserByName(model.RecieverName).Id;

            model.Messages = this.messageService.GetConversationMessages(this.User.Identity.Name, model.RecieverName, model.ShowAll);
            model.RecieverId = recieverId;

            var result = this.PartialView("_ChatViewPartial", model);

            return result;
        }
    }
}