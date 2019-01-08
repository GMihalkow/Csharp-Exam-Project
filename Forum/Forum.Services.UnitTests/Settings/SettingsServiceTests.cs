using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Common;
using Forum.Services.Db;
using Forum.Services.Settings;
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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Forum.Services.UnitTests.Settings
{
    public class SettingsServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly SettingsService settingsService;

        public SettingsServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(databaseName: TestsConstants.InMemoryDbName);

            this.dbContext = new ForumDbContext(options.Options);

            this.dbService = new DbService(dbContext);

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

            this.settingsService = new SettingsService(this.mapper, this.dbService, null, null, null, null, null);
        }

        private void TruncateUsersTable()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);

            this.dbService.DbContext.SaveChanges();
        }

        [Fact]
        public void EditProfile_returns_true_when_correct()
        {
            this.TruncateUsersTable();

            var user = new ForumUser
            {
                UserName = TestsConstants.TestUsername1,
                Location = TestsConstants.TestLocation,
                Gender = TestsConstants.TestGender,
                Id = TestsConstants.TestId
            };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            var model = new EditProfileInputModel { Username = TestsConstants.TestUsername1, Gender = TestsConstants.TestGender, Location = TestsConstants.TestLocation1 };

            var actualResult = this.settingsService.EditProfile(user, model);

            Assert.True(actualResult == true);
        }

        [Fact]
        public void MapEditModel_returns_true_when_correct()
        {
            this.TruncateUsersTable();

            var user = new ForumUser
            {
                UserName = TestsConstants.TestUsername1,
                Location = TestsConstants.TestLocation,
                Gender = TestsConstants.TestGender,
                Id = TestsConstants.TestId
            };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = this.mapper.Map<EditProfileInputModel>(user);

            var actualResult = this.settingsService.MapEditModel(user.UserName);

            Assert.Equal(expectedResult.Username, actualResult.Username);
        }


        [Fact]
        public void BuildFile_returns_true_when_correct()
        {
            this.TruncateUsersTable();

            var user = new ForumUser
            {
                UserName = TestsConstants.TestUsername1,
                Location = TestsConstants.TestLocation,
                Gender = TestsConstants.TestGender,
                Id = TestsConstants.TestId
            };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            var viewModel = this.mapper.Map<UserJsonViewModel>(user);

            var jsonStr = JsonConvert.SerializeObject(viewModel);

            var expectedResult = Encoding.UTF8.GetBytes(jsonStr);

            var claims = new List<Claim>
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.UserName)
            };

            var identity = new ClaimsIdentity(claims, "Test");

            var actualResult = this.settingsService.BuildFile(new ClaimsPrincipal(identity));

            Assert.Equal(expectedResult, actualResult);
        }
    }
}