using AutoMapper;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Message;
using Forum.ViewModels.Interfaces.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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

        public IEnumerable<Models.Message> GetConversationMessages(string firstPersonId, string secondPersonId)
        {
            var conversationMessages =
                this.dbService
                .DbContext
                .Messages
                .Where(m =>
                (m.AuthorId == firstPersonId && m.RecieverId == secondPersonId)
                ||
                (m.AuthorId == secondPersonId && m.RecieverId == firstPersonId))
                .OrderBy(m => m.CreatedOn)
                .ToList();

            return conversationMessages;
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