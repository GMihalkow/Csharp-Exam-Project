using Forum.Models;
using Forum.ViewModels.Interfaces.Post;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Services.Interfaces.Post
{
    public interface IPostService
    {
        IEditPostInputModel GetEditPostModel(string Id, ClaimsPrincipal principal);

        Task AddPost(IPostInputModel model, ForumUser user, string forumId);

        IPostViewModel GetPost(string id);

        string ParseDescription(string description);

        bool DoesPostExist(string Id);

        int ViewPost(string id);

        IEnumerable<ILatestPostViewModel> GetLatestPosts();

        IEnumerable<IPopularPostViewModel> GetPopularPosts();

        int Edit(IEditPostInputModel model);
    }
}