namespace Forum.Web.Services.Contracts
{
    using Forum.Models;
    using Forum.Web.ViewModels.Post;

    public interface IPostService
    {
        void AddPost(PostInputModel model, ForumUser user);
    }
}