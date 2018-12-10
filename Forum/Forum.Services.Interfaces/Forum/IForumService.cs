using Forum.Models;
using Forum.ViewModels.Interfaces.Forum;
using System.Threading.Tasks;

namespace Forum.Services.Interfaces.Forum
{
    public interface IForumService
    {
        void Add(IForumFormInputModel model, string category);

        Task<SubForum> GetPostsByForum(string id);

        SubForum GetForum(string id);
    }
}