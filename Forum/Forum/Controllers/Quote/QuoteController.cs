using Forum.Models;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Quote;
using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Quote;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers.Quote
{
    [Authorize]
    public class QuoteController : BaseController
    {
        private readonly IQuoteService quoteService;
        private readonly IPostService postService;
        private readonly IReplyService replyService;

        public QuoteController(IAccountService accountService, IQuoteService quoteService, IPostService postService, IReplyService replyService) : base(accountService)
        {
            this.quoteService = quoteService;
            this.postService = postService;
            this.replyService = replyService;
        }

        public IActionResult Create(string id)
        {
            var reply = this.replyService.GetReply(id);
            if (reply == null)
            {
                return this.View("Error", new ErrorViewModel { Message = "Invalid reply Id." });
            }

            var recieverName = reply.Author.UserName;

            var model =
                new QuoteInputModel
                {
                    ReplyId = id,
                    Quote = reply.Description,
                    RecieverId = reply.Author.Id,
                };

            this.ViewData["ReplierName"] = reply.Author.UserName;
            this.ViewData["PostName"] = reply.Post.Name;

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(QuoteInputModel model)
        {
            var user = this.accountService.GetUser(this.User);

            var reply = this.replyService.GetReply(model.ReplyId);

            var recieverName = this.accountService.GetUserById(model.RecieverId).UserName;

            model.Description = this.postService.ParseDescription(model.Description);

            this.quoteService.Add(model, user, recieverName);
            
            return this.Redirect($"/Post/Details?id={reply.PostId}");
        }

        public IActionResult Quote(string id)
        {
            var quote = this.quoteService.GetQuote(id);
            if (quote == null)
            {
                return this.View("Error", new ErrorViewModel { Message = "Invalid quote Id." });
            }

            var recieverName = quote.Author.UserName;

            var model =
                new QuoteInputModel
                {
                    ReplyId = quote.ReplyId,
                    Quote = quote.Description,
                    RecieverId = quote.Reply.Author.Id,
                };

            this.ViewData["ReplierName"] = quote.Reply.Author.UserName;
            this.ViewData["PostName"] = quote.Reply.Post.Name;
            this.ViewData["QuoteRecieverId"] = quote.Author.Id;

            return this.View("QuoteAQuoteCreate", model);
        }

        [HttpPost]
        public IActionResult QuoteAQuoteCreate(QuoteInputModel model)
        {
            var user = this.accountService.GetUser(this.User);

            var reply = this.replyService.GetReply(model.ReplyId);

            var recieverName = this.accountService.GetUserById(model.QuoteRecieverId).UserName;

            model.Description = this.postService.ParseDescription(model.Description);

            this.quoteService.Add(model, user, recieverName);

            return this.Redirect($"/Post/Details?id={reply.PostId}");
        }
    }
}