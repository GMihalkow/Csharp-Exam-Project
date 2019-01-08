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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Forum.Services.UnitTests.Report.Post
{
    public class ReplyReportServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly PostReportService postReportService;

        public ReplyReportServiceTests()
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
        
        private void TruncatePostsTable()
        {
            var posts = this.dbService.DbContext.Posts.ToList();
            this.dbService.DbContext.Posts.RemoveRange(posts);

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

        [Fact]
        public void DismissPostReport_returns_correct_result()
        {
            this.TruncatePostReportsTable();
            this.TruncateUsersTable();

            var postReport = new PostReport { Id = TestsConstants.TestId };

            this.dbService.DbContext.PostReports.Add(postReport);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = 1;

            var actualResult = this.postReportService.DismissPostReport(postReport.Id, new ModelStateDictionary());

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void DismissPostReport_returns_zero_results_when_id_is_invalid()
        {
            this.TruncatePostReportsTable();
            this.TruncateUsersTable();

            var expectedResult = 0;

            var actualResult = this.postReportService.DismissPostReport(TestsConstants.TestId, new ModelStateDictionary());

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetPostReports_returns_list_when_correct()
        {
            this.TruncatePostReportsTable();
            this.TruncateUsersTable();

            var postReportsList = new List<PostReport>();

            for (int i = 0; i < 10; i++)
            {
                var postReport = new PostReport();

                this.dbService.DbContext.PostReports.Add(postReport);
                this.dbService.DbContext.SaveChanges();

                postReportsList.Add(postReport);
            }

            var expectedResult = postReportsList.Take(5).Select(p => this.mapper.Map<PostReportViewModel>(p)).ToList();

            var actualResult = this.postReportService.GetPostReports(0);

            Assert.Equal(expectedResult.Count(), actualResult.Count());
        }

        [Fact]
        public void AddPostReport_returns_correct_result()
        {
            this.TruncatePostReportsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var post = new Models.Post { Id = TestsConstants.TestId1 };
            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var author = new ForumUser { Id = TestsConstants.TestId };
            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.SaveChanges();

            var model = new PostReportInputModel { Description = TestsConstants.TestDescription, PostId = post.Id, Title = TestsConstants.TestTitle };

            var expectedResult = model;

            var actualResult = this.postReportService.AddPostReport(model, author.Id);

            Assert.Equal(expectedResult, actualResult);
        }

    }
}