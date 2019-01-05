using Forum.Models;
using Forum.Services.Interfaces.Account;
using Forum.Services.Interfaces.Forum;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Quote;
using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Post;
using Forum.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace Forum.Web.Controllers.Post
{
    [Authorize]
    public class PostController : BaseController
    {
        private readonly IReplyService replyService;
        private readonly IQuoteService quoteService;
        private readonly IForumService forumService;
        private readonly IPostService postService;

        public PostController(IAccountService accountService, IReplyService replyService, IQuoteService quoteService, IForumService forumService, IPostService postService)
            : base(accountService)
        {
            this.replyService = replyService;
            this.quoteService = quoteService;
            this.forumService = forumService;
            this.postService = postService;
        }

        public IActionResult Create(string id)
        {
            SubForum Forum = this.forumService.GetForum(id);

            PostInputModel model = new PostInputModel
            {
                ForumId = Forum.Id,
                ForumName = Forum.Name
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(PostInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                ForumUser user = this.accountService.GetUser(this.User);
                this.postService.AddPost(model, user, model.ForumId).GetAwaiter().GetResult();

                return this.Redirect($"/Forum/Posts?Id={model.ForumId}");
            }
            else
            {
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
            }
        }

        [AllowAnonymous]
        [TypeFilter(typeof(ViewsFilter))]
        public IActionResult Details(string Id, int start)
        {
            var viewModel = this.postService.GetPost(Id, start);
            viewModel.PagesCount = this.postService.GetPagesCount(this.replyService.GetPostRepliesIds(Id).Count());

            this.ViewData["PostId"] = Id;

            this.ViewData["PostReplyIds"] = this.replyService.GetPostRepliesIds(Id).ToList();

            return this.View(viewModel);
        }

        public IActionResult Edit(string Id)
        {
            var postExists = this.postService.DoesPostExist(Id);
            if (!postExists)
            {
                return this.NotFound();
            }

            var viewModel = this.postService.GetEditPostModel(Id, this.User);

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditPostInputModel model)
        {
            var forumsIds = this.forumService.GetAllForums(this.User).Select(f => f.Id);
            if (!forumsIds.Contains(model.ForumId))
            {
                this.ModelState.AddModelError("Invalid forum id", "Invalid forum id");
            }

            if (this.ModelState.IsValid)
            {
                var forum = this.forumService.GetForum(model.ForumId);
                if (forum == null)
                {
                    return this.NotFound();
                }

                this.postService.Edit(model);

                return this.Redirect($"/Post/Details?Id={model.Id}");
            }
            else
            {
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
            }
        }
    }
}