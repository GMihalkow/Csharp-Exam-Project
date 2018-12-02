namespace Forum.Web.Controllers.Category
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Account.Contracts;
    using global::Forum.Services.Category.Contracts;
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
            /*TODO: Creata a separete library for the view models 
            and make controllers to know only about them, not the Forum.Models*/
            if(ModelState.IsValid)
            {
                var user = this.accountService.GetUser(this.User);

                var category = this.mapper.Map<Category>(model);

                var result = this.categoryService.AddCategory(category, user).GetAwaiter().GetResult();

                return this.Redirect("/");
            }
            else
            {
                return this.View(model);
            }
        }
    }
}