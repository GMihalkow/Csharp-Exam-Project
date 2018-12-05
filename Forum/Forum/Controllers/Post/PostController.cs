using Forum.Models;
using Forum.Services.Interfaces.Forum;
using Forum.Services.Interfaces.Post;
using Forum.ViewModels.Post;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forum.Web.Controllers.Post
{
    [Authorize]
    public class PostController : BaseController
    {
        private readonly IForumService forumService;
        private readonly IPostService postService;

        public PostController(IAccountService accountService, IForumService forumService, IPostService postService) 
            : base(accountService)
        {
            this.forumService = forumService;
            this.postService = postService;
        }

        public IActionResult Create(string id)
        {
            SubForum Forum = this.forumService.GetForum(id).GetAwaiter().GetResult();

            PostInputModel model = new PostInputModel
            {
                ForumId = Forum.Id,
                ForumName = Forum.Name
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostInputModel model)
        {
            //Finish create post convertion to tags.
            if (ModelState.IsValid)
            {
                ForumUser user = this.accountService.GetUser(this.User);

                await this.postService.AddPost(model, user, model.ForumId);

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
            viewModel.Description = this.postService.ParseDescription(viewModel.Description);
            this.ViewData["PostId"] = id;
            return this.View(viewModel);
        }
    }
}