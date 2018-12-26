using Forum.ViewModels.Interfaces.Message;
using System.Collections.Generic;
using System.Security.Claims;

namespace Forum.Services.Interfaces.Message
{
    public interface IMessageService
    {
        IEnumerable<Models.Message> GetConversationMessages(string firstPersonId, string secondPersonId);

        int SendMessage(ISendMessageInputModel model, string authorId);
    }
}