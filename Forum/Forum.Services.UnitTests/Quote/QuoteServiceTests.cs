using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Common;
using Forum.Services.Db;
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

namespace Forum.Services.UnitTests.Quote
{
    public class QuoteServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly QuoteService quoteService;

        public QuoteServiceTests()
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
        }

        private void TruncateQuotesTable()
        {
            var quotes = this.dbService.DbContext.Quotes.ToList();
            this.dbService.DbContext.Quotes.RemoveRange(quotes);

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
        public void DeleteUserQuotes_returns_correct_result_when_correct()
        {
            this.TruncateUsersTable();
            this.TruncateQuotesTable();
            this.TruncateForumsTable();
            this.TruncatePostsTable();

            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };

            this.dbService.DbContext.Users.Add(user);
            this.dbService.DbContext.SaveChanges();

            for (int i = 0; i < 5; i++)
            {
                var quote = new Models.Quote { Id = Guid.NewGuid().ToString(), Author = user, AuthorId = user.Id };

                this.dbService.DbContext.Quotes.Add(quote);
                this.dbService.DbContext.SaveChanges();
            }

            var expectedResult = 5;

            var actualResult = this.quoteService.DeleteUserQuotes(user);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetQuote_returns_entity_result_when_correct()
        {
            this.TruncateUsersTable();
            this.TruncateQuotesTable();
            this.TruncateForumsTable();
            this.TruncatePostsTable();

            var quote = new Models.Quote { Id = TestsConstants.TestId };

            this.dbService.DbContext.Quotes.Add(quote);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = quote;

            var actualResult = this.quoteService.GetQuote(quote.Id, new ModelStateDictionary());

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetQuote_returns_null_result_when_incorrect()
        {
            this.TruncateUsersTable();
            this.TruncateQuotesTable();
            this.TruncateForumsTable();
            this.TruncatePostsTable();

            var actualResult = this.quoteService.GetQuote(TestsConstants.TestId, new ModelStateDictionary());

            Assert.Null(actualResult);
        }

        [Fact]
        public void AddQuote_returns_one_result_when_correct()
        {
            this.TruncateUsersTable();
            this.TruncateQuotesTable();
            this.TruncateForumsTable();
            this.TruncatePostsTable();

            var reply = new Models.Reply { Id = TestsConstants.TestId2 };

            var author = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.Replies.Add(reply);
            this.dbService.DbContext.SaveChanges();

            var model = new QuoteInputModel
            {
                Quote = TestsConstants.ParsedValidPostDescription,
                Description = TestsConstants.ValidPostDescription,
                RecieverId = reciever.Id,
                ReplyId = reply.Id
            };

            var expectedResult = 1;

            var actualResult = this.quoteService.AddQuote(model, author, reciever.UserName);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetQuotesByForum_returns_correct_list_when_correct()
        {
            this.TruncateUsersTable();
            this.TruncateQuotesTable();
            this.TruncateForumsTable();
            this.TruncatePostsTable();

            var post = new Models.Post { Id = TestsConstants.TestId };

            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();

            var reply = new Models.Reply { Id = TestsConstants.TestId1, PostId = post.Id, Post = post };

            this.dbService.DbContext.Replies.Add(reply);
            this.dbService.DbContext.SaveChanges();

            var quotesList = new List<Models.Quote>();
            for (int i = 0; i < 5; i++)
            {
                var quote = new Models.Quote { Reply = reply, ReplyId = reply.Id };

                this.dbService.DbContext.Quotes.Add(quote);
                this.dbService.DbContext.SaveChanges();

                quotesList.Add(quote);
            }

            var expectedResult = quotesList.Select(q => this.mapper.Map<QuoteViewModel>(q)).ToList();

            var actualResult = this.quoteService.GetQuotesByPost(post.Id);

            Assert.Equal(expectedResult.Count(), actualResult.Count());
        }
    }
}