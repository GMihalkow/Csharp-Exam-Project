using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Services.Common;
using Forum.Services.Db;
using Forum.Services.Message;
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
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Forum.Services.UnitTests.Message
{
    public class MessageServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly MessageService messageService;

        public MessageServiceTests()
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

            this.messageService = new MessageService(this.mapper, this.dbService);
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

        private void TruncateMessagesTable()
        {
            var messages = this.dbService.DbContext.Messages.ToList();
            this.dbService.DbContext.Messages.RemoveRange(messages);

            this.dbService.DbContext.SaveChanges();
        }

        private void TruncatePostsTable()
        {
            var posts = this.dbService.DbContext.Posts.ToList();
            this.dbService.DbContext.Posts.RemoveRange(posts);

            this.dbService.DbContext.SaveChanges();
        }

        [Fact]
        public void RemoveUserMessages_returns_zero_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateMessagesTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var author = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.SaveChanges();

            var message = new Models.Message { Description = TestsConstants.ValidTestMessageDescription, Id = TestsConstants.TestId, Author = author, AuthorId = author.Id, Reciever = reciever, RecieverId = reciever.Id };

            this.dbService.DbContext.Messages.Add(message);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = 0;
            var actualResult = this.messageService.RemoveUserMessages(author.UserName);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void SendMessage_returns_one_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateMessagesTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var author = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.SaveChanges();

            var message = new SendMessageInputModel { Description = TestsConstants.ValidTestMessageDescription, ShowAll = false, RecieverId = reciever.Id, RecieverName = reciever.NormalizedUserName };

            var expectedResult = 1;
            var actualResult = this.messageService.SendMessage(message, author.Id);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void SendMessage_returns_zero_when_incorrect()
        {
            this.TruncateCategoriesTable();
            this.TruncateMessagesTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var author = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.SaveChanges();

            var message = new SendMessageInputModel { Description = TestsConstants.InvalidTestMessageDescription, ShowAll = false, RecieverId = reciever.Id, RecieverName = reciever.NormalizedUserName };

            var expectedResult = 0;
            var actualResult = this.messageService.SendMessage(message, author.Id);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetRecentConversations_returns_list_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateMessagesTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var author = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.SaveChanges();

            var message = new Models.Message
            {
                Description = TestsConstants.ValidTestMessageDescription,
                Author = author,
                AuthorId = author.Id,
                Reciever = reciever,
                RecieverId = reciever.Id,
                CreatedOn = DateTime.UtcNow
            };

            var message1 = new Models.Message
            {
                Description = TestsConstants.ValidTestMessageDescription,
                Author = reciever,
                AuthorId = reciever.Id,
                Reciever = author,
                RecieverId = author.Id,
                CreatedOn = DateTime.UtcNow
            };

            this.dbService.DbContext.Messages.Add(message);
            this.dbService.DbContext.Messages.Add(message1);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = new List<string> {  reciever.UserName }.ToList();
            var actualResult = this.messageService.GetRecentConversations(author.UserName).OrderBy(n => n).ToList();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetConversationMessages_showall_false_returns_list_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateMessagesTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var author = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.SaveChanges();

            var message = new Models.Message
            {
                Description = TestsConstants.ValidTestMessageDescription,
                Author = author,
                AuthorId = author.Id,
                Reciever = reciever,
                RecieverId = reciever.Id,
                CreatedOn = DateTime.UtcNow
            };

            var message1 = new Models.Message
            {
                Description = TestsConstants.ValidTestMessageDescription,
                Author = reciever,
                AuthorId = reciever.Id,
                Reciever = author,
                RecieverId = author.Id,
                CreatedOn = DateTime.UtcNow
            };

            this.dbService.DbContext.Messages.Add(message);
            this.dbService.DbContext.Messages.Add(message1);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = new List<Models.Message> { message, message1 };
            var actualResult = this.messageService.GetConversationMessages(author.UserName, reciever.UserName, false).ToList();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetConversationMessages_showall_true_returns_list_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateMessagesTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var author = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = new List<Models.Message>();

            for (int i = 0; i < 10; i++)
            {
                var message = new Models.Message
                {
                    Description = TestsConstants.ValidTestMessageDescription,
                    Author = author,
                    AuthorId = author.Id,
                    Reciever = reciever,
                    RecieverId = reciever.Id,
                    CreatedOn = DateTime.UtcNow
                };
                
                this.dbService.DbContext.Messages.Add(message);
                this.dbService.DbContext.SaveChanges();

                expectedResult.Add(message);
            }
            
            var actualResult = this.messageService.GetConversationMessages(author.UserName, reciever.UserName, true).ToList();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetUnreadMessages_returns_correct_count_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateMessagesTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var author = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.SaveChanges();
            
            for (int i = 0; i < 10; i++)
            {
                var message = new Models.Message
                {
                    Description = TestsConstants.ValidTestMessageDescription,
                    Author = author,
                    AuthorId = author.Id,
                    Reciever = reciever,
                    RecieverId = reciever.Id,
                    CreatedOn = DateTime.UtcNow
                };

                this.dbService.DbContext.Messages.Add(message);
                this.dbService.DbContext.SaveChanges();
            }

            var expectedResult = 10;

            var actualResult = this.messageService.GetUnreadMessages(reciever.UserName).Sum(um => um.MessagesCount);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetLatestMessages_returns_empty_list_when_icorrect()
        {
            this.TruncateCategoriesTable();
            this.TruncateMessagesTable();
            this.TruncateUsersTable();
            this.TruncatePostsTable();

            var author = new ForumUser { Id = TestsConstants.TestId1, UserName = TestsConstants.TestUsername1 };
            var reciever = new ForumUser { Id = TestsConstants.TestId2, UserName = TestsConstants.TestUsername2 };

            this.dbService.DbContext.Users.Add(author);
            this.dbService.DbContext.Users.Add(reciever);
            this.dbService.DbContext.SaveChanges();

            var expectedResult = new List<ChatMessageViewModel>();

            var actualResult = this.messageService.GetLatestMessages(TestsConstants.InvalidDateTime, author.UserName, reciever.Id);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}