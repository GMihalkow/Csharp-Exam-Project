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
    using Forum.Services.Interfaces.Post;

    public class HomeController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;

        public HomeController(IAccountService accountService, ICategoryService categoryService, IPostService postService) : base(accountService)
        {
            this.categoryService = categoryService;
            this.postService = postService;
        }

        public IActionResult Index()
        {
            IndexInfoViewModel viewModel = new IndexInfoViewModel
            {
                Categories = this.categoryService.GetUsersCategories().ToArray(),
                TotalUsersCount = this.accountService.GetUsersCount(),
                NewestUser = this.accountService.GetNewestUser(),
                TotalPostsCount = this.accountService.GetTotalPostsCount(),
                LatestPosts = this.postService.GetLatestPosts(),
                PopularPosts = this.postService.GetPopularPosts()
            };

            if (this.User.IsInRole("Administrator") || this.User.IsInRole("Owner"))
            {
                viewModel.Categories = this.categoryService.GetAllCategories().GetAwaiter().GetResult().ToArray();
            }

            return this.View(viewModel);
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
            string result = path;
            if (this.Request.Query.ContainsKey("?id"))
            {
                result = result + "?id=" + this.Request.Query["?id"];
            }

            return this.Redirect(result);
        }

        public IActionResult AcceptConsent()
        {
            this.Response.Cookies.Append("GDPR", "true", new CookieOptions { Path = "/", Expires = DateTime.UtcNow.AddDays(3), HttpOnly = false, IsEssential = true });

            return this.Redirect("/");
        }
    }
}