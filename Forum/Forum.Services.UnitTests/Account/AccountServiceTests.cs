using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Account;
using Forum.Services.Common;
using Forum.Services.Common.Comparers;
using Forum.Services.Db;
using Forum.Services.Interfaces.Profile;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace Forum.Services.UnitTests.Account
{
    public class AccountServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly Mock<IProfileService> profileService;

        private readonly AccountService accountService;

        public AccountServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(databaseName: TestsConstants.InMemoryDbName);

            this.dbContext = new ForumDbContext(options.Options);

            this.dbService = new DbService(dbContext);

            this.profileService = new Mock<IProfileService>();

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

            this.accountService = new AccountService(this.mapper, this.dbService, null, null, this.profileService.Object);
        }

        private void TruncateUsersTable()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);

            this.dbService.DbContext.SaveChanges();
        }

        private void TruncateRolesTable()
        {
            var roles = this.dbService.DbContext.Roles.ToList();
            this.dbService.DbContext.Roles.RemoveRange(roles);

            this.dbService.DbContext.SaveChanges();
        }

        private void TruncateUserRolesTable()
        {
            var userRoles = this.dbService.DbContext.UserRoles.ToList();
            this.dbService.DbContext.UserRoles.RemoveRange(userRoles);

            this.dbService.DbContext.SaveChanges();
        }

        [Fact]
        public void UsernameExists_returns_true_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername1, Id = Guid.NewGuid().ToString() });
            this.dbService.DbContext.SaveChanges();

            Assert.True(this.accountService.UsernameExists(TestsConstants.TestUsername1) == true);
        }

        [Fact]
        public void UsernameExists_returns_false_when_incorrect()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            Assert.True(this.accountService.UsernameExists(TestsConstants.TestUsername2) == false);
        }

        [Fact]
        public void UserExists_returns_true_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername1, Id = Guid.NewGuid().ToString() });
            this.dbService.DbContext.SaveChanges();

            Assert.True(this.accountService.UserExists(TestsConstants.TestUsername1) == true);
        }

        [Fact]
        public void UserExists_returns_false_when_incorrect()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            Assert.True(this.accountService.UserExists(TestsConstants.TestUsername2) == false);
        }

        [Fact]
        public void GetUserById_returns_user_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            var id = Guid.NewGuid().ToString();

            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername1, Id = id });
            this.dbService.DbContext.SaveChanges();

            Assert.Equal(this.accountService.GetUserById(id.ToString(), new ModelStateDictionary()).Id, new ForumUser() { UserName = TestsConstants.TestUsername1, Id = id }.Id);
        }

        [Fact]
        public void GetUserById_returns_null_when_incorrect()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername1, Id = Guid.NewGuid().ToString() });
            this.dbService.DbContext.SaveChanges();

            Assert.Null(this.accountService.GetUserById(TestsConstants.TestId, new ModelStateDictionary()));
        }

        [Fact]
        public void GetUserByName_returns_user_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            var id = Guid.NewGuid().ToString();

            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername1, Id = id });
            this.dbService.DbContext.SaveChanges();

            Assert.Equal(this.accountService.GetUserByName(TestsConstants.TestUsername1, new ModelStateDictionary()).UserName, new ForumUser() { UserName = TestsConstants.TestUsername1, Id = id }.UserName);
        }

        [Fact]
        public void GetUserByName_returns_null_when_incorrect()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername1, Id = Guid.NewGuid().ToString() });
            this.dbService.DbContext.SaveChanges();

            Assert.Null(this.accountService.GetUserByName(TestsConstants.TestUsername2, new ModelStateDictionary()));
        }

        [Fact]
        public void GetUsernames_returns_correct_list_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername1, Id = Guid.NewGuid().ToString() });
            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername2, Id = Guid.NewGuid().ToString() });
            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername3, Id = Guid.NewGuid().ToString() });
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<string> { TestsConstants.TestUsername1, TestsConstants.TestUsername2, TestsConstants.TestUsername3 }.OrderBy(n => n).ToList();
            var actualList = this.accountService.GetUsernames().OrderBy(n => n).ToList();

            Assert.Equal(expectedList, actualList);
        }

        [Fact]
        public void GetUsernames_returns_empty_list_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            Assert.Equal(new List<string> { }, this.accountService.GetUsernames());
        }

        [Fact]
        public void GetUsers_returns_correct_list_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername1, Id = "testId1" });
            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername2, Id = "testId2" });
            this.dbService.DbContext.Users.Add(new ForumUser() { UserName = TestsConstants.TestUsername3, Id = "testId3" });
            this.dbService.DbContext.SaveChanges();

            var actualList = this.accountService.GetUsers().OrderBy(u => u.UserName);

            var expectedList = new List<ForumUser>
            {
                new ForumUser() { UserName = TestsConstants.TestUsername1, Id = TestsConstants.TestId1 },
                new ForumUser() { UserName = TestsConstants.TestUsername2, Id = TestsConstants.TestId2 },
                new ForumUser() { UserName = TestsConstants.TestUsername3, Id = TestsConstants.TestId3 }
            }.OrderBy(u => u.UserName);

            Assert.Equal(expectedList, actualList, new IdComparer());
        }

        [Fact]
        public void GetUsers_returns_empty_list_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            Assert.Equal(new List<ForumUser>(), new List<ForumUser>());
        }

        [Fact]
        public void GetNewestUser_returns_username_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            for (int i = 0; i < 5; i++)
            {
                var user = new ForumUser { UserName = TestsConstants.TestUsername1 };
                user.RegisteredOn = DateTime.UtcNow.AddDays(i);

                this.dbService.DbContext.Users.Add(user);
                this.dbService.DbContext.SaveChanges();
            }

            var expectedResult = TestsConstants.TestUsername1;

            var actualResult = this.accountService.GetNewestUser();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetNewestUser_returns_empty_string_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = string.Empty;

            var actualResult = this.accountService.GetNewestUser();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmailExists_returns_true_string_when_correct()
        {
            var users = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(users);
            this.dbService.DbContext.SaveChanges();

            var user = new ForumUser { Email = TestsConstants.TestEmail };
            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            var actualResult = this.accountService.EmailExists(TestsConstants.TestEmail).GetAwaiter().GetResult();

            Assert.True(actualResult == true);
        }

        [Fact]
        public void GetUsernamesWithourOwner_returns_correct_list_with_entities()
        {
            this.TruncateRolesTable();
            this.TruncateUsersTable();

            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };
            var secondUser = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername2 };
            var thirdUser = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername3 };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.Users.Add(secondUser);
            this.dbService.DbContext.Users.Add(thirdUser);
            this.dbService.DbContext.SaveChanges();

            var ownerRole = new IdentityRole { Id = TestsConstants.TestId1, Name = Common.Role.Owner };
            var userRole = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Common.Role.User };

            this.dbService.DbContext.Roles.Add(ownerRole);
            this.dbService.DbContext.Roles.Add(userRole);
            this.dbService.DbContext.SaveChanges();

            var testSecondUserRole = new IdentityUserRole<string> { RoleId = userRole.Id, UserId = secondUser.Id };
            var testThirdUserRole = new IdentityUserRole<string> { RoleId = userRole.Id, UserId = thirdUser.Id };

            this.dbService.DbContext.UserRoles.Add(testSecondUserRole);
            this.dbService.DbContext.UserRoles.Add(testThirdUserRole);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Roles.Add(ownerRole);
            this.dbService.DbContext.SaveChanges();

            var roleId = ownerRole.Id;
            var userId = user.Id;

            var newUserRole = new IdentityUserRole<string> { RoleId = roleId, UserId = userId };
            this.dbService.DbContext.UserRoles.Add(newUserRole);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = new List<string> { secondUser.UserName, thirdUser.UserName }.OrderBy(n => n);

            var actualResult = this.accountService.GetUsernamesWithoutOwner().OrderBy(n => n);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}