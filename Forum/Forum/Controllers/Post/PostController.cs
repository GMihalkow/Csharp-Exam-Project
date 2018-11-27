namespace Forum.Web.Controllers.Post
{
    using global::Forum.Models;
    using global::Forum.Web.Services.Contracts;
    using global::Forum.Web.ViewModels.Post;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
                
                this.postService.AddPost(model, user);

                return this.Redirect($"/Forum/Posts?Id={model.ForumId}");
            }
            else
            {
                return this.View(model);
            }
        }
    }
}