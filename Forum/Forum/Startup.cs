using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Forum.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Forum.Web;
using Forum.Web.Middlewares;
using Forum.Services.Account;
using AutoMapper;
using Forum.Services.Forum;
using Forum.Services.Post.Contracts;
using Forum.Web.Services.Contracts;
using Forum.Services.Category.Contracts;
using Forum.Services.Category;
using Forum.Services.Forum.Contracts;
using Forum.Services.Db;

namespace Forum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddDbContext<ForumDbContext>(options =>
                options
                   .UseSqlServer(
                     Configuration.GetConnectionString("DefaultConnection")));
            services
                .AddIdentity<ForumUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;

                    options.User.RequireUniqueEmail = true;
                })
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ForumDbContext>();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddRazorPagesOptions(options =>
            {
                options.AllowAreas = true;
                options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Account/Login";
                options.LogoutPath = $"/Account/Logout";
                options.AccessDeniedPath = $"/Account/AccessDenied";
            });

            //AntiforgeryToken 
            services.AddAntiforgery();

            //Registrating services
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IForumService, ForumService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<DbService>();
            services.AddScoped<IUserClaimsPrincipalFactory<ForumUser>, UserClaimsPrincipalFactory<ForumUser, IdentityRole>>();
            services.AddAutoMapper();

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Admin");
                    });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMiddleware(typeof(SeedRolesMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}