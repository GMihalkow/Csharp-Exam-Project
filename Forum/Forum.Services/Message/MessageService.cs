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

        public IEnumerable<Models.Message> GetConversationMessages(string firstPersonName, string secondPersonName)
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
                .OrderBy(m => m.CreatedOn)
                .ToList();

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
                  .Select(m => this.mapper.Map<ChatMessageViewModel>(m))
                  .OrderBy(m => m.CreatedOn)
                  .ToList();

                foreach (var message in firstMessages)
                {
                    message.LoggedInUser = loggedInUser;
                }

                return firstMessages;
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
                .Select(m => this.mapper.Map<ChatMessageViewModel>(m))
                .OrderBy(m => m.CreatedOn)
                .ToList();

            foreach (var message in messages)
            {
                message.LoggedInUser = loggedInUser;
            }

            return messages;
        }

        public IEnumerable<string> GetRecentConversations(string username)
        {
            var recentSentMessages =
                this.dbService
                .DbContext
                .Messages
                .Include(m => m.Author)
                .Include(m => m.Reciever)
                .Where(m => (m.Author.UserName == username))
                .OrderByDescending(m => m.CreatedOn)
                .Take(2)
                .Select(m => m.Reciever.UserName)
                .Distinct()
                .ToList();

            var recentRecievedMessages =
                this.dbService
                .DbContext
                .Messages
                .Include(m => m.Author)
                .Include(m => m.Reciever)
                .Where(m => (m.Reciever.UserName == username))
                .OrderByDescending(m => m.CreatedOn)
                .Take(3)
                .Select(m => m.Author.UserName)
                .ToList();

            var recentConversations =
                recentSentMessages
                .Concat(recentRecievedMessages)
                .Distinct()
                .ToList();

            return recentConversations;
        }

        public int SendMessage(ISendMessageInputModel model, string authorId)
        {
            var message = new Models.Message
            {
                RecieverId = model.RecieverId,
                Description = model.Description,
                CreatedOn = DateTime.UtcNow,
                AuthorId = authorId
            };

            this.dbService.DbContext.Messages.Add(message);
            return this.dbService.DbContext.SaveChanges();
        }
    }
}