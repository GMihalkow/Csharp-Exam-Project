using Forum.Models;
using Forum.Services.Interfaces.Forum;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Quote;
using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Post;
using Forum.Web.Filters;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            if (ModelState.IsValid)
            {
                ForumUser user = this.accountService.GetUser(this.User);
                this.postService.AddPost(model, user, model.ForumId).GetAwaiter().GetResult();

                return this.Redirect($"/Forum/Posts?Id={model.ForumId}");
            }
            else
            {
                return this.View(model);
            }
        }

        [AllowAnonymous]
        [TypeFilter(typeof(ViewsFilter))]
        public IActionResult Details(string Id, int start)
        {
            var viewModel = this.postService.GetPost(Id, start);
            viewModel.Description = this.postService.ParseDescription(viewModel.Description);
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
            var forum = this.forumService.GetForum(model.ForumId);
            if(forum == null)
            {
                return this.NotFound();
            }

            this.postService.Edit(model);

            return this.Redirect($"/Post/Details?Id={model.Id}");
        }
    }
}