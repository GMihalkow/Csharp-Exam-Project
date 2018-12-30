using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum.Web.Areas.Owner.ViewModels.Role
{
    public class UserRoleViewModel : IMapFrom<IdentityUserRole<string>>
    {
        public string UserId { get; set; }

        public ForumUser User { get; set; }

        public string RoleId { get; set; }

        public IdentityRole Role { get; set; }
    }
}