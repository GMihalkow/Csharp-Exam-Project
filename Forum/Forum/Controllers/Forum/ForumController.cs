namespace Forum.Web.Controllers.Forum
{
    using global::Forum.Web.Services.Contracts;
    using global::Forum.Web.ViewModels.Forum;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize("Admin")]
    public class ForumController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IForumService forumService;

        public ForumController(IAccountService accountService, ICategoryService categoryService, IForumService forumService)
            : base(accountService)
        {
            this.categoryService = categoryService;
            this.forumService = forumService;
        }

        public IActionResult Create()
        {
            var names = this.categoryService.GetCategoriesNames();

            ForumFormInputModel model = new ForumFormInputModel
            {
                Categories = names
            };
            
            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(ForumFormInputModel model)
        {
            if(ModelState.IsValid)
            {
                this.forumService.Add(model);

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
            var model = this.forumService.GetPostsByForum(id);

            return this.View(model);
        }
    }
}