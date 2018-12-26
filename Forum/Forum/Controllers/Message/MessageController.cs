using Forum.Services.Interfaces.Message;
using Forum.ViewModels.Message;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers.Message
{
    [Authorize]
    public class MessageController : BaseController
    {
        private readonly IMessageService messageService;

        public MessageController(IAccountService accountService, IMessageService messageService) : base(accountService)
        {
            this.messageService = messageService;
        }

        [HttpPost]
        public PartialViewResult Send([FromBody] SendMessageInputModel model)
        {
            var authorId = this.accountService.GetUser(this.User).Id;

            this.messageService.SendMessage(model, authorId);

            var firstUserId = this.accountService.GetUser(this.User).Id;

            var secondUserId = "f9b494b3-83cf-435d-b4dc-ae7e562e38c3";

            var viewModel = new SendMessageInputModel
            {
                Messages = this.messageService.GetConversationMessages(firstUserId, secondUserId),
                RecieverId = secondUserId
            };

            return this.PartialView("../Account/_ChatViewPartial", viewModel);
        }
    }
}