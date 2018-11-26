namespace Forum.Web.Controllers
{
    using global::Forum.Web.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        protected readonly IAccountService accountService;

        public BaseController(IAccountService accountService)
        {
            //TODO: get insparation from other sites for design, specially for forms
            this.accountService = accountService;
        }
    }
}