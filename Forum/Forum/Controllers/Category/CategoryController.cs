namespace Forum.Web.Controllers.Category
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Category.Contracts;
    using global::Forum.Web.Services.Contracts;
    using global::Forum.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize("Admin")]
    public class CategoryController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;

        public CategoryController(IMapper mapper, IAccountService accountService, ICategoryService categoryService) 
            : base(accountService)
        {
            this.mapper = mapper;
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
                var user = this.accountService.GetUser(this.User);

                var category = this.mapper.Map<Category>(model);

                this.categoryService.AddCategory(category, user);

                return this.Redirect("/");
            }
            else
            {
                return this.View(model);
            }
        }
    }
}