using Forum.Models;

namespace Forum.Services.Forum.Contracts
{
    public interface IForumService
    {
        void Add(SubForum model, string category);

        SubForum GetPostsByForum(string id);

        SubForum GetForum(string id);
    }
}