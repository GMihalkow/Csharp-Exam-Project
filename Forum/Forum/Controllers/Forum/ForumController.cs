namespace Forum.Web.Controllers.Forum
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Account.Contracts;
    using global::Forum.Services.Category.Contracts;
    using global::Forum.Services.Forum.Contracts;
    using global::Forum.Web.ViewModels.Forum;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize("Admin")]
    public class ForumController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;
        private readonly IForumService forumService;

        public ForumController(IMapper mapper, IAccountService accountService, ICategoryService categoryService, IForumService forumService)
            : base(accountService)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
            this.forumService = forumService;
        }

        public IActionResult Create()
        {
            var names =  this.categoryService.GetCategoriesNames();

            ForumFormInputModel model = new ForumFormInputModel
            {
                Categories = names.GetAwaiter().GetResult()
            };
            
            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(ForumFormInputModel model)
        {
            if(ModelState.IsValid)
            {
                var forum =
                    this.mapper
                    .Map<SubForum>(model);

                this.forumService.Add(forum, model.ForumModel.Category);

                return this.Redirect("/");
            }
            else
            {
                return this.View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult Posts(string id)
        {
            var forum = this.forumService.GetPostsByForum(id);

            var model = new ForumPostsInputModel
            {
                Forum = forum,
                Posts = forum.Posts
            };
            
            return this.View(model);
        }
    }
}