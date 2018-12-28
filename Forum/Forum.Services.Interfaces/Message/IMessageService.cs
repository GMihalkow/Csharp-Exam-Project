using Forum.ViewModels.Interfaces.Message;
using System.Collections.Generic;
using System.Security.Claims;

namespace Forum.Services.Interfaces.Message
{
    public interface IMessageService
    {
        IEnumerable<Models.Message> GetConversationMessages(string firstPersonName, string secondPersonName);

        int SendMessage(ISendMessageInputModel model, string authorId);

        IEnumerable<IChatMessageViewModel> GetLatestMessages(string lastDate, string loggedInUser);
    }
}