﻿using Forum.Web.Areas.Owner.Services.Contracts;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Areas.Owner.Controllers.Role
{
    [Area("Owner")]
    [Authorize(Common.Role.Owner)]
    [Route("[area]/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;
        private readonly IAccountService accountService;

        public RoleController(IRoleService roleService, IAccountService accountService)
        {
            this.roleService = roleService;
            this.accountService = accountService;
        }

        public IActionResult Index()
        {
            var usersRoles = this.roleService.GetUsersRoles();

            this.ViewData["usernames"] = this.accountService.GetUsernamesWithoutOwner();

            return this.View(usersRoles);
        }

        [HttpGet("Promote/id={id}")]
        public IActionResult Promote(string id)
        {
            var user = this.accountService.GetUserById(id);
            if (user == null)
            {
                //TODO: Add error
            }

            this.roleService.Promote(user);

            return this.Redirect("/Owner/Role");
        }

        [HttpGet("Demote/id={id}")]
        public IActionResult Demote(string id)
        {
            var user = this.accountService.GetUserById(id);
            if (user == null)
            {
                //TODO: Add error
            }

            this.roleService.Demote(user);

            return this.Redirect("/Owner/Role");
        }

        [HttpGet("Search")]
        public PartialViewResult Search(string key)
        {
            var usersRoles = this.roleService.SearchForUsers(key);

            return this.PartialView("_EditRolesTablePartial", usersRoles);
        }
    }
}