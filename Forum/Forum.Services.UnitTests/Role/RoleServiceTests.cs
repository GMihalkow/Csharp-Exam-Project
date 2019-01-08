using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Account;
using Forum.Services.Common;
using Forum.Services.Db;
using Forum.Services.Role;
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
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Forum.Services.UnitTests.Role
{
    public class RoleServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly RoleService roleService;

        public RoleServiceTests()
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

            this.roleService = new RoleService(this.dbService, this.mapper, null, null);
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
        public void GetUsersRoles_returns_correct_list_with_entities()
        {
            this.TruncateRolesTable();
            this.TruncateUserRolesTable();
            this.TruncateUsersTable();

            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };
            var secondUser = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername2 };
            var thirdUser = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername3 };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.Users.Add(secondUser);
            this.dbService.DbContext.Users.Add(thirdUser);
            this.dbService.DbContext.SaveChanges();

            var ownerRole = new IdentityRole { Id = TestsConstants.TestId1, Name = Common.Role.Owner };
            var adminRole = new IdentityRole { Id = TestsConstants.TestId2, Name = Common.Role.Administrator };
            var userRole = new IdentityRole { Id = TestsConstants.TestId3, Name = Common.Role.User };

            this.dbService.DbContext.Roles.Add(ownerRole);
            this.dbService.DbContext.Roles.Add(adminRole);
            this.dbService.DbContext.Roles.Add(userRole);
            this.dbService.DbContext.SaveChanges();

            for (int i = 0; i < this.dbService.DbContext.Users.Count(); i++)
            {
                var currentRoleId = this.dbService.DbContext.Roles.ToList()[i].Id;
                var currentuserId = this.dbService.DbContext.Users.ToList()[i].Id;

                var newUserRole = new IdentityUserRole<string> { RoleId = currentRoleId, UserId = currentuserId };
                this.dbService.DbContext.UserRoles.Add(newUserRole);
                this.dbService.DbContext.SaveChanges();
            }

            var expectedResult =
                this.dbService
                .DbContext
                .UserRoles
                .Where(ur => ur.RoleId != ownerRole.Id)
                .Select(ur => this.mapper.Map<UserRoleViewModel>(ur))
                .ToList();

            foreach (var ur in expectedResult)
            {
                ur.User = this.dbService.DbContext.Users.FirstOrDefault(u => u.Id == ur.UserId);
                ur.Role = this.dbService.DbContext.Roles.Where(r => r.Id == ur.RoleId).FirstOrDefault();
            }

            expectedResult =
                expectedResult
                .Where(ur => ur.Role.Name != Common.Role.Owner)
                .OrderBy(ur => ur.User.UserName)
                .Skip(0)
                .Take(5)
                .ToList();

            var actualResult = this.roleService.GetUsersRoles(0);

            Assert.Equal(expectedResult.Count(), actualResult.Count());
        }

        [Fact]
        public void SearchForUsers_returns_correct_list_with_entities()
        {
            this.TruncateRolesTable();
            this.TruncateUserRolesTable();
            this.TruncateUsersTable();

            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };
            var secondUser = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername2 };
            var thirdUser = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername3 };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.Users.Add(secondUser);
            this.dbService.DbContext.Users.Add(thirdUser);
            this.dbService.DbContext.SaveChanges();

            var adminRole = new IdentityRole { Id = TestsConstants.TestId2, Name = Common.Role.Administrator };

            this.dbService.DbContext.Roles.Add(adminRole);
            this.dbService.DbContext.SaveChanges();

            var userRole = new IdentityUserRole<string> { RoleId = adminRole.Id, UserId = user.Id };

            this.dbService.DbContext.UserRoles.Add(userRole);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = new List<UserRoleViewModel> { new UserRoleViewModel { User = user, UserId = user.Id, Role = adminRole, RoleId = adminRole.Id} };

            var actualResult = this.roleService.SearchForUsers("g");

            Assert.Equal(expectedResult.First().UserId, actualResult.First().UserId);
        }
    }
}