namespace Forum.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Forum.Web.Controllers;
    using Forum.Web.ViewModels.Home;
    using Forum.Models;
    using System.Linq;
    using Forum.Services.Category.Contracts;
    using Forum.Services.Account.Contracts;

    public class HomeController : BaseController
    {
        private readonly ICategoryService categoryService;

        public HomeController(IAccountService accountService, ICategoryService categoryService) : base(accountService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            Category[] categories = new Category[0];

            if (this.User.IsInRole("Admin"))
            {
                categories = categories.Concat(this.categoryService.GetAllCategories()).ToArray();
            }
            else
            {
                categories = categories.Concat(this.categoryService.GetUsersCategories()).ToArray();
            }

            IndexInfoViewModel viewModel = new IndexInfoViewModel
            {
                Categories = categories,
                TotalUsersCount = this.accountService.GetUsersCount(),
                NewestUser = this.accountService.GetNewestUser(),
                TotalPostsCount = this.accountService.GetTotalPostsCount()
            };

            return View(viewModel);
        }
    }
}