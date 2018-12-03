namespace Forum.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Forum.Web.Controllers;
    using Forum.Web.ViewModels.Home;
    using System.Linq;
    using Forum.Web.Services.Account.Contracts;
    using Forum.Services.Interfaces.Category;

    public class HomeController : BaseController
    {
        private readonly ICategoryService categoryService;

        public HomeController(IAccountService accountService, ICategoryService categoryService) : base(accountService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {

            IndexInfoViewModel viewModel = new IndexInfoViewModel
            {
                Categories = this.categoryService.GetUsersCategories().ToArray(),
                TotalUsersCount = this.accountService.GetUsersCount(),
                NewestUser = this.accountService.GetNewestUser(),
                TotalPostsCount = this.accountService.GetTotalPostsCount()
            };
            
            if (this.User.IsInRole("Admin"))
            {
                viewModel.Categories.Concat(this.categoryService.GetAllCategories().GetAwaiter().GetResult().ToArray());
            }
            
            return View(viewModel);
        }
    }
}