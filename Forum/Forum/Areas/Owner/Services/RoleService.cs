using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Forum.Services.Interfaces.Db;
using Forum.Web.Areas.Owner.Services.Contracts;
using Forum.Web.Areas.Owner.ViewModels.Role;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Areas.Owner.Services
{
    public class RoleService : IRoleService
    {
        private readonly IDbService dbService;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;

        public RoleService(IDbService dbService, IMapper mapper, IAccountService accountService)
        {
            this.dbService = dbService;
            this.mapper = mapper;
            this.accountService = accountService;
        }

        public IEnumerable<UserRoleViewModel> GetUsersRoles()
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

            return usersRoles;
        }
    }
}