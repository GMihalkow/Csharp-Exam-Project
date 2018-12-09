namespace Forum.Web.Utilities
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using System;
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

        public static async Task SeedThemes(HttpContext httpContext)
        {
            if (!httpContext.Request.Cookies.ContainsKey("Theme"))
            {
                httpContext.Response.Cookies.Append("Theme", "dark", new CookieOptions { Expires = DateTime.UtcNow.AddDays(3), Path = "/" });
            }
            else
            {
                if (httpContext.Request.Cookies["Theme"] != "dark" &&
                    httpContext.Request.Cookies["Theme"] != "light")
                {
                    httpContext.Response.Cookies.Delete("Theme");
                    httpContext.Response.Cookies.Append("Theme", "dark", new CookieOptions { Expires = DateTime.UtcNow.AddDays(3), Path = "/" });
                    httpContext.Response.Redirect(httpContext.Request.Path);
                }
            }
        }
    }
}