namespace Forum.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Forum.Web.Controllers;
    using Forum.Web.ViewModels.Home;
    using System.Linq;
    using Forum.Web.Services.Account.Contracts;
    using Forum.Services.Interfaces.Category;
    using Microsoft.AspNetCore.Http;
    using System;

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

            if (this.User.IsInRole("Administrator"))
            {
                viewModel.Categories.Concat(this.categoryService.GetAllCategories().GetAwaiter().GetResult().ToArray());
            }

            return View(viewModel);
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult ChangeTheme(string theme, string path)
        {
            if ((this.HttpContext.Request.Cookies.ContainsKey("Theme")))
            {
                this.HttpContext.Response.Cookies.Delete("Theme");
                this.HttpContext.Response.Cookies.Append("Theme", theme, new CookieOptions { Expires = DateTime.UtcNow.AddDays(3), Path = "/" });
            }
            else
            {
                this.HttpContext.Response.Cookies.Append("Theme", theme, new CookieOptions { Expires = DateTime.UtcNow.AddDays(3), Path = "/" });
            }
            string result = path + "?id=" + this.Request.Query["?id"];

            return this.Redirect(result);
        }
    }
}