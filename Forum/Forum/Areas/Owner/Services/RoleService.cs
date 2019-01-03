using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Forum.Models;
using Forum.Services.Interfaces.Db;
using Forum.Web.Areas.Owner.Services.Contracts;
using Forum.Web.Areas.Owner.ViewModels.Role;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Forum.Web.Areas.Owner.Services
{
    public class RoleService : IRoleService
    {
        private readonly IDbService dbService;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;
        private readonly UserManager<ForumUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleService(IDbService dbService, IMapper mapper, IAccountService accountService, UserManager<ForumUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbService = dbService;
            this.mapper = mapper;
            this.accountService = accountService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IEnumerable<UserRoleViewModel> GetUsersRoles(int start)
        {
            var usersRoles =
                this.dbService
                .DbContext
                .UserRoles
                .Select(ur => this.mapper.Map<UserRoleViewModel>(ur))
                .ToList();

            foreach (var userRole in usersRoles)
            {
                userRole.User = this.accountService.GetUserById(userRole.UserId);
                userRole.Role = this.dbService.DbContext.Roles.Where(r => r.Id == userRole.RoleId).FirstOrDefault();
            }

            usersRoles = 
                usersRoles
                .Where(ur => ur.Role.Name != Common.Role.Owner)
                .OrderBy(ur => ur.User.UserName)
                .Skip(start)
                .Take(5)
                .ToList();

            return usersRoles;
        }
        public int Demote(ForumUser user)
        {
            this.userManager.RemoveFromRoleAsync(user, Common.Role.Administrator).GetAwaiter().GetResult();
            this.userManager.AddToRoleAsync(user, Common.Role.User).GetAwaiter().GetResult();

            return this.dbService.DbContext.SaveChanges();
        }

        public int Promote(ForumUser user)
        {
            this.userManager.RemoveFromRoleAsync(user, Common.Role.User).GetAwaiter().GetResult();
            this.userManager.AddToRoleAsync(user, Common.Role.Administrator).GetAwaiter().GetResult();

            return this.dbService.DbContext.SaveChanges();
        }

        public IEnumerable<UserRoleViewModel> SearchForUsers(string str)
        {
            var usersRoles =
                this.dbService
                .DbContext
                .UserRoles
                .Select(ur => this.mapper.Map<UserRoleViewModel>(ur))
                .ToList();

            foreach (var userRole in usersRoles)
            {
                userRole.User = this.accountService.GetUserById(userRole.UserId);
                userRole.Role = this.dbService.DbContext.Roles.Where(r => r.Id == userRole.RoleId).FirstOrDefault();
            }

            usersRoles = 
                usersRoles
                .Where(ur => ur.Role.Name != Common.Role.Owner && ur.User.UserName.ToLower().StartsWith(str) || ur.User.UserName == str)
                .OrderBy(ur => ur.User.UserName)
                .ToList();

            return usersRoles;
        }
    }
}