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
using Forum.Services.Forum;
using Forum.Services.Post.Contracts;
using Forum.Services.Category;
using Forum.Services.Db;
using Forum.Web.ViewModels.Account;
using Forum.MapConfiguration;
using Forum.Web.Services.Account.Contracts;
using Forum.Web.Services.Account;
using Forum.ViewModels.Forum;
using Forum.ViewModels.Post;
using Forum.Services.Interfaces.Category;
using Forum.Services.Interfaces.Forum;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Db;
using Forum.ViewModels.Category;
using Forum.Services.Reply;
using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Reply;
using Forum.ViewModels.Quote;
using Forum.Services.Interfaces.Quote;
using Forum.Services.Quote;
using Forum.ViewModels.Report;
using Forum.Services.Report;
using Forum.Services.Interfaces.Report;

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
            //TODO: Use Coverlet for code coverage.
            //TODO: Add logging.
            //TODO: Extract views to a razor view class library.
            //TODO: Extract logout view to a modal.
            //TODO: Create method in service should return the model so you can test it
            //TODO: Add Cookie consent.
            //TODO: Validate that names of entities are unique
            //TODO: Break down site.js file and use it only when neccessary
            //TODO: Implement change password functionality.
            //TODO: Look at the dota forum and change up the design.
            //TODO: Look at the presentation for Advanced topics and follow the GDPR slide for the GDPR
            //TODO: Extract model binding validation magic strings
            //TODO: Create areas
            //TODO: Use Cloudinary for pictures and save the link to the image in the db
            //TODO: Extract magic strings in private consts where needed, 
            //and extract to a global constants class if they are used in more places

            var config = AutoMapperConfig.RegisterMappings(
                 typeof(LoginUserInputModel).Assembly,
                 typeof(RegisterUserViewModel).Assembly,
                 typeof(CategoryInputModel).Assembly,
                 typeof(ForumFormInputModel).Assembly,
                 typeof(ForumInputModel).Assembly,
                 typeof(ForumPostsInputModel).Assembly,
                 typeof(PostInputModel).Assembly,
                 typeof(ReplyInputModel).Assembly,
                 typeof(PostViewModel).Assembly,
                 typeof(ReplyViewModel).Assembly,
                 typeof(EditProfileInputModel).Assembly,
                 typeof(QuoteInputModel).Assembly,
                 typeof(PostReportInputModel).Assembly,
                 typeof(ReplyReportInputModel).Assembly,
                 typeof(QuoteReportInputModel).Assembly);

            var mapper = config.CreateMapper();

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
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReplyService, ReplyService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IDbService, DbService>();
            services.AddScoped<IUserClaimsPrincipalFactory<ForumUser>, UserClaimsPrincipalFactory<ForumUser, IdentityRole>>();

            //Registrating the automapper
            services.AddSingleton(mapper);

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrator");
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

            //Custom middlewares
            app.UseMiddleware(typeof(SeedRolesMiddleware));
            app.UseMiddleware(typeof(ThemesMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}