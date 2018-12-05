namespace Forum.Web.Utilities
{
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public static class Seeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            bool AdminRoleExists = await roleManager.RoleExistsAsync("Administrator");
            if (!AdminRoleExists)
            {
                var adminRole = new IdentityRole() { Name = "Administrator", NormalizedName = "ADMINISTRATOR", ConcurrencyStamp = "0" };
                var result = await roleManager.CreateAsync(adminRole);
            }

            bool UserRoleExists = await roleManager.RoleExistsAsync("User");
            if (!UserRoleExists)
            {
                var userRole = new IdentityRole() { Name = "User", NormalizedName = "USER", ConcurrencyStamp = "1" };
                var result = await roleManager.CreateAsync(userRole);
            }
        }
    }
}