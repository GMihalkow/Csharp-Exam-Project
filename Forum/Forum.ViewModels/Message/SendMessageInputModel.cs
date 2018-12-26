using System.Collections.Generic;
using Forum.ViewModels.Interfaces.Message;

namespace Forum.ViewModels.Message
{
    public class SendMessageInputModel : ISendMessageInputModel
    {
        public string Description { get; set; }

        public string RecieverId { get; set; }

        public string RecieverName { get; set; }

        public IEnumerable<Models.Message> Messages { get; set; }
    }
}