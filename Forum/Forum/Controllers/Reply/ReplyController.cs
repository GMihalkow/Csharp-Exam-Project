using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Reply;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers.Reply
{
    public class ReplyController : BaseController
    {
        private readonly IReplyService replyService;

        public ReplyController(IAccountService accountService, IReplyService replyService)
            : base(accountService)
        {
            this.replyService = replyService;
        }

        [HttpPost]
        public IActionResult Create(ReplyInputModel model)
        {
            var user = this.accountService.GetUser(this.User);

            this.replyService.Add(model, user).GetAwaiter().GetResult();

            return this.Redirect($"/Post/Details?id={model.PostId}");
        }
    }
}