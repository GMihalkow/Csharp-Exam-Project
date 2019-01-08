using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Category;
using Forum.Services.Common;
using Forum.Services.Db;
using Forum.Services.Forum;
using Forum.Services.Post;
using Forum.Services.Quote;
using Forum.Services.Reply;
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

namespace Forum.Services.UnitTests.Reply
{
    public class ReplyServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly ForumService forumService;

        private readonly CategoryService categoryService;

        private readonly PostService postService;

        private readonly ReplyService replyService;

        private readonly QuoteService quoteService;

        public ReplyServiceTests()
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

            this.replyService = new ReplyService(this.mapper, this.dbService, this.postService);
        }

        private void TruncateRepliesTable()
        {
            var replies = this.dbService.DbContext.Replies.ToList();
            this.dbService.DbContext.Replies.RemoveRange(replies);

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
        public void DeleteUserReplies_returns_correct_result_when_correct()
        {
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();
            this.TruncateRepliesTable();

            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            for (int i = 0; i < 5; i++)
            {
                var reply = new Models.Reply { Author = user, AuthorId = user.Id };

                this.dbService.DbContext.Replies.Add(reply);
                this.dbService.DbContext.SaveChanges();
            }

            var expectedResult = 5;

            var actualResult = this.replyService.DeleteUserReplies(user.UserName);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetReply_returns_entity_result_when_correct()
        {
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();
            this.TruncateRepliesTable();

            var reply = new Models.Reply { Id = TestsConstants.TestId };

            this.dbService.DbContext.Replies.Add(reply);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = reply;

            var actualResult = this.replyService.GetReply(reply.Id, new ModelStateDictionary());

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetReply_returns_null_result_when_incorrect()
        {
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();
            this.TruncateRepliesTable();

            var actualResult = this.replyService.GetReply(TestsConstants.TestId, new ModelStateDictionary());

            Assert.Null(actualResult);
        }

        [Fact]
        public void AddReply_returns_entity_result_when_correct()
        {
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();
            this.TruncateRepliesTable();

            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            var post = new Models.Post { Author = user, Description = TestsConstants.ValidPostDescription, AuthorId = user.Id, Id = TestsConstants.TestId3 }; ;

            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var model = new ReplyInputModel { Description = TestsConstants.ValidPostDescription, Author = user, PostId = post.Id };

            var expectedResult = 1;

            var actualResult = this.replyService.AddReply(model, user).GetAwaiter().GetResult();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GePostRepliesIds_returns_correct_list_when_correct()
        {
            this.TruncateForumsTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();
            this.TruncateRepliesTable();

            var post = new Models.Post { Id = TestsConstants.TestId1 };

            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var reply = new Models.Reply { Id = TestsConstants.TestId, PostId = post.Id, Post = post };

            this.dbService.DbContext.Replies.Add(reply);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = new List<string> { reply.Id };

            var actualResult = this.replyService.GetPostRepliesIds(post.Id);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}