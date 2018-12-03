namespace Forum.Services.Interfaces.Post
{
    using global::Forum.Models;
    using global::Forum.ViewModels.Interfaces.Post;

    public interface IPostService
    {
        void AddPost(IPostInputModel model, ForumUser user, string forumId);
    }
}