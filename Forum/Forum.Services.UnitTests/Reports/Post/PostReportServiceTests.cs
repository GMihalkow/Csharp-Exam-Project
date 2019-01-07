using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Common;
using Forum.Services.Db;
using Forum.Services.Report.Post;
using Forum.ViewModels.Account;
using Forum.ViewModels.Category;
using Forum.ViewModels.Forum;
using Forum.ViewModels.Message;
using Forum.ViewModels.Post;
using Forum.ViewModels.Profile;
using Forum.ViewModels.Quote;
using Forum.ViewModels.Reply;
using Forum.ViewModels.Report;
using Forum.ViewModels.Role;
using Forum.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Forum.Services.UnitTests.Report.Post
{
    public class PostReportServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly PostReportService postReportService;

        public PostReportServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(databaseName: TestsConstants.InMemoryDbName);

            this.dbContext = new ForumDbContext(this.options.Options);

            this.dbService = new DbService(this.dbContext);

            this.mapper = AutoMapperConfig.RegisterMappings(
               typeof(LoginUserInputModel).Assembly,
               typeof(EditPostInputModel).Assembly,
               typeof(RegisterUserViewModel).Assembly,
               typeof(CategoryInputModel).Assembly,
               typeof(UserJsonViewModel).Assembly,
               typeof(ForumFormInputModel).Assembly,
               typeof(ForumInputModel).Assembly,
               typeof(RecentConversationViewModel).Assembly,
               typeof(ForumPostsInputModel).Assembly,
               typeof(PostInputModel).Assembly,
               typeof(LatestPostViewModel).Assembly,
               typeof(ProfileInfoViewModel).Assembly,
               typeof(PopularPostViewModel).Assembly,
               typeof(ReplyInputModel).Assembly,
               typeof(PostViewModel).Assembly,
               typeof(ReplyViewModel).Assembly,
               typeof(EditProfileInputModel).Assembly,
               typeof(SendMessageInputModel).Assembly,
               typeof(QuoteInputModel).Assembly,
               typeof(PostReportInputModel).Assembly,
               typeof(ReplyReportInputModel).Assembly,
               typeof(UserRoleViewModel).Assembly,
               typeof(ChatMessageViewModel).Assembly,
               typeof(QuoteReportInputModel).Assembly)
               .CreateMapper();

            this.postReportService = new PostReportService(this.mapper, this.dbService);
        }
        
        private void TruncateUsersTable()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);

            this.dbService.DbContext.SaveChanges();
        }

        private void TruncatePostReportsTable()
        {
            var postReports = this.dbService.DbContext.PostReports.ToList();
            this.dbService.DbContext.PostReports.RemoveRange(postReports);

            this.dbService.DbContext.SaveChanges();
        }

        [Fact]
        public void GetPostReportsCount_returns_correct_result()
        {
            this.TruncatePostReportsTable();
            this.TruncateUsersTable();

            for (int i = 0; i < 5; i++)
            {
                var postReport = new PostReport();

                this.dbService.DbContext.PostReports.Add(postReport);
                this.dbService.DbContext.SaveChanges();
            }

            var expectedResult = 5;

            var actualResult = this.postReportService.GetPostReportsCount();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}