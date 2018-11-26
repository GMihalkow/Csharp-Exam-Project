namespace Forum.Web.Controllers.Category
{
    using Forum.Models;
    using Forum.Web.Services.Contracts;
    using Forum.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize("Admin")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(IAccountService accountService, ICategoryService categoryService) 
            : base(accountService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CategoryInputModel model)
        {
            if(ModelState.IsValid)
            {
                ForumUser user = this.accountService.GetUser(this.User);

                this.categoryService.AddCategory(model, user);

                return this.Redirect("/");
            }
            else
            {
                return this.View(model);
            }
        }
    }
}