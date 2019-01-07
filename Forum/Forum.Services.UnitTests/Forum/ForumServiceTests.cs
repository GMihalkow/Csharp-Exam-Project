using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Models.Enums;
using Forum.Services.Category;
using Forum.Services.Common;
using Forum.Services.Common.Comparers;
using Forum.Services.Db;
using Forum.Services.Forum;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace Forum.Services.UnitTests.Forum
{
    public class ForumServiceTests
    {
        private readonly HttpContextAccessor httpContext;

        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly ForumService forumService;

        private readonly CategoryService categoryService;

        public ForumServiceTests()
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

            this.categoryService = new CategoryService(this.mapper, this.dbService);

            this.forumService = new ForumService(this.mapper, this.dbService, this.categoryService);

            this.httpContext = new HttpContextAccessor();
        }

        private void TruncateCategoriesTable()
        {
            var categories = this.dbService.DbContext.Categories.ToList();
            this.dbService.DbContext.Categories.RemoveRange(categories);

            this.dbService.DbContext.SaveChanges();
        }

        private void TruncateUsersTable()
        {
            var categories = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(categories);

            this.dbService.DbContext.SaveChanges();
        }

        private void TruncateForumsTable()
        {
            var forums = this.dbService.DbContext.Forums.ToList();
            this.dbService.DbContext.Forums.RemoveRange(forums);

            this.dbService.DbContext.SaveChanges();
        }

        private void TruncatePostsTable()
        {
            var posts = this.dbService.DbContext.Posts.ToList();
            this.dbService.DbContext.Posts.RemoveRange(posts);

            this.dbService.DbContext.SaveChanges();
        }

        [Fact]
        public void GetForum_returns_entity_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var forum = new SubForum { Name = TestsConstants.ValidForumName, Id = TestsConstants.TestId };
            this.dbService.DbContext.Forums.Add(forum);

            this.dbService.DbContext.SaveChanges();

            var expectedId = forum.Id;

            var actualId = this.forumService.GetForum(forum.Id, new ModelStateDictionary()).Id;

            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void GetForum_returns_null_when_incorrect()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var actualEntity = this.forumService.GetForum(TestsConstants.TestId, new ModelStateDictionary());

            Assert.Null(actualEntity);
        }

        [Fact]
        public void GetPostsByForum_returns_empty_list_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var forum = new SubForum { Id = TestsConstants.TestId1, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>() };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();

            var post = new Models.Post { Id = TestsConstants.TestId, Name = TestsConstants.ValidPostName, Forum = forum, ForumId = forum.Id };
            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<Models.Post> { post };

            var actualList = this.forumService.GetPostsByForum(forum.Id, 0);

            Assert.Equal(expectedList, actualList);
        }

        [Fact]
        public void GetPostsByForum_returns_entities_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var forum = new SubForum { Id = TestsConstants.TestId1, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>() };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<Models.Post> { };

            var actualList = this.forumService.GetPostsByForum(forum.Id, 0);

            Assert.Equal(expectedList, actualList);
        }

        [Fact]
        public void AddForum_returns_one_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var category = new Models.Category { Id = TestsConstants.TestId, Name = TestsConstants.ValidCategoryName };
            this.dbService.DbContext.Categories.Add(category);
            this.dbService.DbContext.SaveChanges();

            var model = new ForumFormInputModel();
            model.ForumModel.Name = TestsConstants.ValidForumName;
            model.ForumModel.Category = category.Name;

            var expectedResult = 1;

            var actualResult = this.forumService.AddForum(model, category.Id);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Edit_returns_one_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var category = new Models.Category { Id = TestsConstants.TestId1, Name = TestsConstants.ValidCategoryName, Forums = new List<SubForum>() };
            this.dbService.DbContext.Categories.Add(category);
            this.dbService.DbContext.SaveChanges();

            var forum = new SubForum { Name = TestsConstants.ValidForumName, Id = TestsConstants.TestId, Description = "TEST", Category = category, CategoryId = category.Id };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();

            var model = this.mapper.Map<ForumInputModel>(forum);
            model.Category = category.Id;

            var expectedResult = 1;

            var actualResult = this.forumService.Edit(model, forum.Id);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Delete_returns_one_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var forum = new SubForum { Name = TestsConstants.ValidForumName, Id = TestsConstants.TestId };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = 1;

            var actualResult = this.forumService.Delete(forum);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ForumExists_returns_true_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var forum = new SubForum { Name = TestsConstants.ValidForumName, Id = TestsConstants.TestId };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();

            var actualResult = this.forumService.ForumExists(forum.Name);

            Assert.True(actualResult == true);
        }

        [Fact]
        public void ForumExists_returns_false_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var actualResult = this.forumService.ForumExists(TestsConstants.ValidForumName);

            Assert.True(actualResult == false);
        }

        [Fact]
        public void GetForumPostsIds_returns_ids_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var forum = new SubForum { Id = TestsConstants.TestId1, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>() };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();

            var post = new Models.Post { Id = TestsConstants.TestId, Name = TestsConstants.ValidPostName, Forum = forum, ForumId = forum.Id };
            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<string> { post.Id };

            var actualList = this.forumService.GetForumPostsIds(forum.Id);

            Assert.Equal(expectedList, actualList);
        }

        [Fact]
        public void GetAllForums_returns_entities_when_Owner_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var category = new Models.Category { Id = TestsConstants.TestId, Name = TestsConstants.ValidCategoryName, Type = CategoryType.AdminOnly };
            this.dbService.DbContext.Categories.Add(category);
            this.dbService.DbContext.SaveChanges();

            var forum = new SubForum { Id = TestsConstants.TestId1, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>(), Category = category, CategoryId = category.Id };
            var secondForum = new SubForum { Id = TestsConstants.TestId2, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>(), Category = category, CategoryId = category.Id };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.Forums.Add(secondForum);
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<SubForum> { forum, secondForum };

            var claims = new List<Claim>
            {
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", Common.Role.Owner)
            };

            var identity = new ClaimsIdentity(claims, "Test");

            var actualList = this.forumService.GetAllForums(new ClaimsPrincipal(identity));

            Assert.Equal(expectedList, actualList);
        }

        [Fact]
        public void GetAllForums_returns_entities_when_User_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var category = new Models.Category { Id = TestsConstants.TestId, Name = TestsConstants.ValidCategoryName, Type = CategoryType.Public };
            this.dbService.DbContext.Categories.Add(category);
            this.dbService.DbContext.SaveChanges();

            var forum = new SubForum { Id = TestsConstants.TestId1, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>(), Category = category, CategoryId = category.Id };
            var secondForum = new SubForum { Id = TestsConstants.TestId2, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>(), Category = category, CategoryId = category.Id };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.Forums.Add(secondForum);
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<SubForum> { forum, secondForum }.OrderBy(f => f.Id);

            var claims = new List<Claim>
            {
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", Common.Role.User)
            };

            var identity = new ClaimsIdentity(claims, "Test");

            var actualList = this.forumService.GetAllForums(new ClaimsPrincipal(identity)).OrderBy(f => f.Id);

            Assert.Equal(expectedList, actualList);
        }


        [Fact]
        public void GetAllForumsIds_returns_entities_when_Owner_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var category = new Models.Category { Id = TestsConstants.TestId, Name = TestsConstants.ValidCategoryName, Type = CategoryType.AdminOnly };
            this.dbService.DbContext.Categories.Add(category);
            this.dbService.DbContext.SaveChanges();

            var forum = new SubForum { Id = TestsConstants.TestId1, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>(), Category = category, CategoryId = category.Id };
            var secondForum = new SubForum { Id = TestsConstants.TestId2, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>(), Category = category, CategoryId = category.Id };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.Forums.Add(secondForum);
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<string> { forum.Id, secondForum.Id }.OrderBy(id => id);

            var claims = new List<Claim>
            {
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", Common.Role.Owner)
            };

            var identity = new ClaimsIdentity(claims, "Test");

            var actualList = this.forumService.GetAllForumsIds(new ClaimsPrincipal(identity), new ModelStateDictionary(), forum.Id).OrderBy(id => id);

            Assert.Equal(expectedList, actualList);
        }


        [Fact]
        public void GetMappedForumModel_returns_entities_when_Owner_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var forum = new SubForum { Id = TestsConstants.TestId1, Name = TestsConstants.ValidForumName, Posts = new List<Models.Post>()};
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = this.mapper.Map<ForumInputModel>(forum);

            var actualResult = this.forumService.GetMappedForumModel(forum).ForumModel;

            Assert.Equal(expectedResult.Name, actualResult.Name);
        }
    }
}