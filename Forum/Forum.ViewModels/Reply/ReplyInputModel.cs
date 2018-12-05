using Forum.Models;
using Forum.ViewModels.Interfaces.Reply;

namespace Forum.ViewModels.Reply
{
    public class ReplyInputModel : IReplyInputModel
    {
        public string Id { get; set; }

        public ForumUser Author { get; set; }

        public string Description { get; set; }

        public string PostId { get; set; }
    }
}