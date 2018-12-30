using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Forum.ViewModels.Interfaces.Message;

namespace Forum.ViewModels.Message
{
    public class SendMessageInputModel : ISendMessageInputModel
    {
        [MinLength(1)]
        public string Description { get; set; }

        public string RecieverId { get; set; }

        public string RecieverName { get; set; }

        public IEnumerable<Models.Message> Messages { get; set; }
    }
}