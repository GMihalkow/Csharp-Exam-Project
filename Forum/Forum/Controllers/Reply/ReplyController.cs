using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Reply;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            if (ModelState.IsValid)
            {
                var user = this.accountService.GetUser(this.User);

                this.replyService.Add(model, user).GetAwaiter().GetResult();

                return this.Redirect($"/Post/Details?id={model.PostId}");
            }
            else
            {
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.NotFound;

                return result;
            }
        }
    }
}