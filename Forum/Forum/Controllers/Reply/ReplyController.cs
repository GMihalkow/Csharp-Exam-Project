using Forum.Models;
using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Reply;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Text;

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
                var sb = new StringBuilder();
                foreach (var value in ModelState.Values)
                {
                    if (value.ValidationState == ModelValidationState.Invalid)
                    {
                        sb.AppendLine(string.Join(Environment.NewLine, value.Errors.Select(e => e.ErrorMessage)));
                    }
                }

                return this.View("Error", new ErrorViewModel { Message = sb.ToString().TrimEnd() });
            }
        }
    }
}