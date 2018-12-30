using Forum.Web.Areas.Owner.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Areas.Owner.Controllers.Role
{
    [Area("Owner")]
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        public IActionResult Index()
        {
            var usersRoles = this.roleService.GetUsersRoles();

            return this.View(usersRoles);
        }
    }
}