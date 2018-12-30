using Forum.Web.Areas.Owner.ViewModels.Role;
using System.Collections.Generic;

namespace Forum.Web.Areas.Owner.Services.Contracts
{
    public interface IRoleService
    {
        IEnumerable<UserRoleViewModel> GetUsersRoles();
    }
}