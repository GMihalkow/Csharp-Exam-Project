using Forum.Models;
using Forum.ViewModels.Interfaces.Post;
using System.Threading.Tasks;

namespace Forum.Services.Interfaces.Post
{
    public interface IPostService
    {
        Task AddPost(IPostInputModel model, ForumUser user, string forumId);

        IPostViewModel GetPost(string id);

        string ParseDescription(string description);

        bool DoesPostExist(string Id);

        int ViewPost(string id);
    }
}