using Forum.Models;
using Forum.ViewModels.Interfaces.Post;

namespace Forum.Services.Interfaces.Post
{
    public interface IPostService
    {
        void AddPost(IPostInputModel model, ForumUser user, string forumId);

        IPostViewModel GetPost(string id);

        string ParseDescription(string description);
    }
}