using Forum.Models;
using Forum.ViewModels.Interfaces.Forum;
using System;
using System.Threading.Tasks;

namespace Forum.Services.Interfaces.Forum
{
    public interface IForumService
    {
        void Add(IForumFormInputModel model, string category);

        Task<SubForum> GetPostsByForum(string Id);

        SubForum GetForum(string Id);

        void Edit(IForumInputModel model, string forumId);

        IForumFormInputModel GetMappedForumModel(SubForum forum);

        void Delete(SubForum forum);
    }
}