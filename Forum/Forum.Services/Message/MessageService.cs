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

        public IEnumerable<IChatMessageViewModel> GetLatestMessages(string lastDate, string loggedInUser)
        {
            //TODO: fix the messages to be between 2 users
            var parsedLastDate = DateTime.Parse(lastDate);

            var messages =
                this.dbService
                .DbContext
                .Messages
                .Include(m => m.Author)
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