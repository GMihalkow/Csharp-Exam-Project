using Forum.Models;
using Forum.ViewModels.Interfaces.Reply;
using System.Threading.Tasks;

namespace Forum.Services.Interfaces.Reply
{
    public interface IReplyService
    {
        Task<int> Add(IReplyInputModel model, ForumUser user);

        Models.Reply GetReply(string id);

        int DeleteUserReplies(string username);
    }
}