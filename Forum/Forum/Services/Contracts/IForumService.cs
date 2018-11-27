namespace Forum.Web.Services.Contracts
{
    using Forum.Models;
    using Forum.Web.ViewModels.Forum;
    using System.Collections.Generic;

    public interface IForumService
    {
        void Add(ForumFormInputModel model);

        ForumPostsInputModel GetPostsByForum(string id);

        SubForum GetForum(string id);
    }
}