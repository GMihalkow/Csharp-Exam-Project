namespace Forum.Web.Controllers.Category
{
    using global::Forum.Services.Common;
    using global::Forum.Services.Interfaces.Account;
    using global::Forum.Services.Interfaces.Category;
    using global::Forum.ViewModels.Category;
    using global::Forum.Web.Attributes.CustomAuthorizeAttributes;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    [AuthorizeRoles(Role.Administrator, Role.Owner)]
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
            /*TODO: Creata a separete library for the view models 
            and make controllers to know only about them, not the Forum.Models*/
            if(ModelState.IsValid)
            {
                var user = this.accountService.GetUser(this.User);
                
                var result = this.categoryService.AddCategory(model, user).GetAwaiter().GetResult();

                return this.Redirect("/");
            }
            else
            {
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
            }
        }
    }
}