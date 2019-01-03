using AutoMapper;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Message;
using Forum.ViewModels.Interfaces.Message;
using Forum.ViewModels.Message;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Forum.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;

        public MessageService(IMapper mapper, IDbService dbService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
        }

        public IEnumerable<Models.Message> GetConversationMessages(string firstPersonName, string secondPersonName, bool showAll)
        {
            var conversationMessages =
                this.dbService
                .DbContext
                .Messages
                .Include(m => m.Author)
                .Include(m => m.Reciever)
                .Where(m =>
                (m.Author.UserName == firstPersonName && m.Reciever.UserName == secondPersonName)
                ||
                (m.Author.UserName == secondPersonName && m.Reciever.UserName == firstPersonName))
                .OrderByDescending(m => m.CreatedOn)
                .ToList();

            foreach (var message in conversationMessages)
            {
                if (message.Reciever.UserName == firstPersonName)
                {
                    message.Seen = true;
                }
            }

            this.dbService.DbContext.SaveChanges();

            if (showAll == true)
            {
                conversationMessages =
                    conversationMessages
                    .OrderBy(m => m.CreatedOn)
                    .ToList();
            }
            else
            {
                conversationMessages =
                    conversationMessages
                    .Take(5)
                    .OrderBy(m => m.CreatedOn)
                    .ToList();
            }

            return conversationMessages;
        }

        public IEnumerable<IChatMessageViewModel> GetLatestMessages(string lastDate, string loggedInUser, string otherUserId)
        {
            var IsDate = DateTime.TryParse(lastDate, out DateTime parsedLastDate);
            if (!IsDate)
            {
                var firstMessages =
                  this.dbService
                  .DbContext
                  .Messages
                  .Include(m => m.Author)
                  .Include(m => m.Reciever)
                  .Where(m =>
                  (m.Author.UserName == loggedInUser && m.RecieverId == otherUserId)
                  ||
                  (m.AuthorId == otherUserId && m.Reciever.UserName == loggedInUser))
                  .ToList();

                foreach (var message in firstMessages)
                {
                    if (message.Reciever.UserName == loggedInUser)
                    {
                        message.Seen = true;
                    }
                }

                this.dbService.DbContext.SaveChanges();

                var messagesViewModel =
                  firstMessages
                  .Select(m => this.mapper.Map<ChatMessageViewModel>(m))
                  .OrderBy(m => m.CreatedOn)
                  .ToList();

                foreach (var message in messagesViewModel)
                {
                    message.LoggedInUser = loggedInUser;
                }

                return messagesViewModel;
            }

            var messages =
                this.dbService
                .DbContext
                .Messages
                .Include(m => m.Author)
                .Include(m => m.Reciever)
                .Where(m =>
                (m.Author.UserName == loggedInUser && m.RecieverId == otherUserId)
                ||
                (m.AuthorId == otherUserId && m.Reciever.UserName == loggedInUser))
                .Where(m => m.CreatedOn.ToString("F") != parsedLastDate.ToString("F") && DateTime.Compare(m.CreatedOn, parsedLastDate) > 0)
                .ToList();

            foreach (var message in messages)
            {
                if (message.Reciever.UserName == loggedInUser)
                {
                    message.Seen = true;
                }
            }

            this.dbService.DbContext.SaveChanges();

            var filteredMessagesViewModel =
                messages
                .Select(m => this.mapper.Map<ChatMessageViewModel>(m))
                .OrderBy(m => m.CreatedOn)
                .ToList();

            foreach (var message in filteredMessagesViewModel)
            {
                message.LoggedInUser = loggedInUser;
            }

            return filteredMessagesViewModel;
        }

        public IEnumerable<string> GetRecentConversations(string username)
        {
            var recentRecievedMessages =
                this.dbService
                .DbContext
                .Messages
                .Include(m => m.Author)
                .Include(m => m.Reciever)
                .Where(m => (m.Reciever.UserName == username) || (m.Author.UserName == username))
                .OrderByDescending(m => m.CreatedOn)
                .Take(5)
                .Select(m => m.Author.UserName)
                .Where(m => m != null)
                .Distinct()
                .ToList();

            return recentRecievedMessages;
        }

        public IEnumerable<IUnreadMessageViewModel> GetUnreadMessages(string username)
        {
            var unreadMessages = new List<UnreadMessageViewModel>();

            var unreadMessagesAuthors =
                this.dbService
                .DbContext
                .Messages
                .Include(m => m.Author)
                .Include(m => m.Reciever)
                .Where(m => m.Reciever.UserName == username)
                .Where(m => m.Seen == false)
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.Author.UserName)
                .Distinct()
                .Take(5)
                .ToList();

            foreach (var authorName in unreadMessagesAuthors)
            {
                var messagesCount =
                    this.dbService
                    .DbContext
                    .Messages
                    .Include(m => m.Author)
                    .Include(m => m.Reciever)
                    .Where(m => m.Author.UserName == authorName && m.Reciever.UserName == username)
                    .Where(m => m.Seen == false)
                    .Count();

                var unreadMessage = new UnreadMessageViewModel
                {
                    AuthorName = authorName,
                    MessagesCount = messagesCount
                };

                unreadMessages.Add(unreadMessage);
            }

            return unreadMessages;
        }

        public void RemoveUserMessages(string username)
        {
            var messages =
                this.dbService
                .DbContext
                .Messages
                .Include(m => m.Author)
                .Include(m => m.Reciever)
                .Where(m => m.Author.UserName == username || m.Reciever.UserName == username)
                .ToList();

            this.dbService.DbContext.SaveChanges();
        }

        public int SendMessage(ISendMessageInputModel model, string authorId)
        {
            var message = new Models.Message
            {
                RecieverId = model.RecieverId,
                Description = model.Description,
                CreatedOn = DateTime.UtcNow,
                AuthorId = authorId,
                Seen = false
            };

            this.dbService.DbContext.Messages.Add(message);
            return this.dbService.DbContext.SaveChanges();
        }
    }
}