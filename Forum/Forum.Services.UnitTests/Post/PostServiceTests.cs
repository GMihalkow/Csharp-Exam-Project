using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Category;
using Forum.Services.Common;
using Forum.Services.Common.Comparers;
using Forum.Services.Db;
using Forum.Services.Forum;
using Forum.Services.Post;
using Forum.Services.Quote;
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
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Forum.Services.UnitTests.Post
{
    public class PostServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly ForumService forumService;

        private readonly PostService postService;

        private readonly QuoteService quoteService;

        private readonly CategoryService categoryService;

        public PostServiceTests()
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

            this.quoteService = new QuoteService(this.mapper, this.dbService);

            this.categoryService = new CategoryService(this.mapper, this.dbService);

            this.forumService = new ForumService(this.mapper, this.dbService, this.categoryService);

            this.postService = new PostService(this.mapper, this.quoteService, this.dbService, this.forumService);
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
        public void DoesPostExist_returns_true_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var post = new Models.Post { Name = TestsConstants.ValidPostName, Id = TestsConstants.TestId };
            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var actualResult = this.postService.DoesPostExist(post.Id);
            Assert.True(actualResult == true);
        }

        [Fact]
        public void DoesPostExist_returns_false_when_incorrect()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var actualResult = this.postService.DoesPostExist(TestsConstants.TestId);

            Assert.True(actualResult == false);
        }

        [Fact]
        public void GetTotalPostsCount_returns_correct_result_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            for (int i = 0; i < 10; i++)
            {
                var post = new Models.Post { Name = TestsConstants.ValidPostName, Id = Guid.NewGuid().ToString() };
                this.dbService.DbContext.Posts.Add(post);
                this.dbService.DbContext.SaveChanges();
            }

            var expectedResult = 10;

            var actualResult = this.postService.GetTotalPostsCount();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ViewPost_increments_post_views_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var post = new Models.Post { Name = TestsConstants.ValidPostName, Id = TestsConstants.TestId };
            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = 1;

            var actualResult = this.postService.ViewPost(post.Id);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void AddPost_returns_one_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var user = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            var forum = new SubForum { Id = TestsConstants.TestId, Posts = new List<Models.Post>() };
            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();

            var model = new PostInputModel { ForumId = forum.Id, Description = TestsConstants.ValidPostDescription, ForumName = forum.Name, Name = TestsConstants.ValidPostName };

            var expectedResult = 1;

            var actualResult = this.postService.AddPost(model, user, forum.Id).GetAwaiter().GetResult();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ParseDescription_returns_parsed_tags_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var exampleDescription = TestsConstants.ValidPostDescription;

            var expectedResult = TestsConstants.ParsedValidPostDescription;

            var actualResult = this.postService.ParseDescription(exampleDescription);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetPost_returns_entity_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var post = new Models.Post { Name = TestsConstants.ValidPostName, Id = TestsConstants.TestId };
            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = this.mapper.Map<PostViewModel>(post);

            var actualResult = this.postService.GetPost(post.Id, 0, new ModelStateDictionary());

            Assert.Equal(expectedResult.Id, actualResult.Id);
        }

        [Fact]
        public void GetLatestPosts_returns_correct_list_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var postsList = new List<Models.Post>();

            for (int i = 0; i < 10; i++)
            {
                var post = new Models.Post { Name = TestsConstants.ValidPostName, Id = Guid.NewGuid().ToString() };
                post.StartedOn = DateTime.UtcNow.AddDays(i);

                postsList.Add(post);

                this.dbService.DbContext.Posts.Add(post);
                this.dbService.DbContext.SaveChanges();
            }

            postsList = postsList.OrderByDescending(p => p.StartedOn).ToList();

            var expectedResult = postsList.Select(p => this.mapper.Map<LatestPostViewModel>(p)).Take(3).Select(p => p.StartedOn).ToList();

            var actualResult = this.postService.GetLatestPosts().Select(p => p.StartedOn).ToList();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetPopularPosts_returns_correct_list_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var postsList = new List<Models.Post>();

            for (int i = 0; i < 10; i++)
            {
                var post = new Models.Post { Name = TestsConstants.ValidPostName, Id = Guid.NewGuid().ToString() };
                post.Views = i;

                postsList.Add(post);

                this.dbService.DbContext.Posts.Add(post);
                this.dbService.DbContext.SaveChanges();
            }

            postsList = postsList.OrderByDescending(p => p.Views).ToList();

            var expectedResult = postsList.Select(p => this.mapper.Map<PopularPostViewModel>(p)).Take(3).Select(p => p.Views).ToList();

            var actualResult = this.postService.GetPopularPosts().Select(p => p.Views).ToList();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Edit_returns_one_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var post = new Models.Post { Name = TestsConstants.ValidPostName, Id = TestsConstants.TestId };
            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var model = new EditPostInputModel { Description = TestsConstants.ValidPostDescription, Id = TestsConstants.TestId };

            var expectedResult = 1;

            var actualResult = this.postService.Edit(model);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}