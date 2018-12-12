using Forum.Models;
using Forum.Services.Interfaces.Forum;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Quote;
using Forum.ViewModels.Post;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Web.Controllers.Post
{
    [Authorize]
    public class PostController : BaseController
    {
        private readonly IQuoteService quoteService;
        private readonly IForumService forumService;
        private readonly IPostService postService;

        public PostController(IAccountService accountService, IQuoteService quoteService, IForumService forumService, IPostService postService)
            : base(accountService)
        {
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
                model.Description = this.postService.ParseDescription(model.Description);
                this.postService.AddPost(model, user, model.ForumId).GetAwaiter().GetResult();

                return this.Redirect($"/Forum/Posts?Id={model.ForumId}");
            }
            else
            {
                return this.View(model);
            }
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.postService.GetPost(id);
            
            this.ViewData["PostId"] = id;
            return this.View(viewModel);
        }
    }
}