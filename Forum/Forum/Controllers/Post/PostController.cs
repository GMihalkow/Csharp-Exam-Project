namespace Forum.Web.Controllers.Post
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Account.Contracts;
    using global::Forum.Services.Forum.Contracts;
    using global::Forum.Services.Post.Contracts;
    using global::Forum.Web.ViewModels.Post;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PostController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IForumService forumService;
        private readonly IPostService postService;

        public PostController(IMapper mapper, IAccountService accountService, IForumService forumService, IPostService postService) 
            : base(accountService)
        {
            this.mapper = mapper;
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
            //Finish create post convertion to tags.
            if (ModelState.IsValid)
            {
                ForumUser user = this.accountService.GetUser(this.User);

                var post = this.mapper.Map<Post>(model);

                this.postService.AddPost(post, user, model.ForumId);

                return this.Redirect($"/Forum/Posts?Id={model.ForumId}");
            }
            else
            {
                return this.View(model);
            }
        }
    }
}