using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Common;
using Forum.Services.Db;
using Forum.Services.Report;
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

namespace Forum.Services.UnitTests.Reports
{
    public class ReportsServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly ReportService reportService;

        public ReportsServiceTests()
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

            this.reportService = new ReportService(this.mapper, this.dbService);
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

        private void TruncateQuoteReportsTable()
        {
            var quoteReports = this.dbService.DbContext.QuoteReports.ToList();
            this.dbService.DbContext.QuoteReports.RemoveRange(quoteReports);

            this.dbService.DbContext.SaveChanges();
        }
        
        private void TruncateReplyReportsTable()
        {
            var replyReports = this.dbService.DbContext.ReplyReports.ToList();
            this.dbService.DbContext.ReplyReports.RemoveRange(replyReports);

            this.dbService.DbContext.SaveChanges();
        }

        [Fact]
        public void DeleteUserReports_returns_correct_result_when_correct()
        {
            this.TruncatePostReportsTable();
            this.TruncateQuoteReportsTable();
            this.TruncateReplyReportsTable();
            this.TruncateUsersTable();

            var user = new ForumUser { Id = TestsConstants.TestId };
            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            var replyReport = new ReplyReport { Author = user, AuthorId = user.Id };
            var postReport = new PostReport { Author = user, AuthorId = user.Id };
            var quoteReport = new QuoteReport { Author = user, AuthorId = user.Id };

            this.dbService.DbContext.ReplyReports.Add(replyReport);
            this.dbService.DbContext.PostReports.Add(postReport);
            this.dbService.DbContext.QuoteReports.Add(quoteReport);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = 3;

            var actualResult = this.reportService.DeleteUserReports(user.UserName);

            Assert.Equal(expectedResult, actualResult);
        }

    }
}