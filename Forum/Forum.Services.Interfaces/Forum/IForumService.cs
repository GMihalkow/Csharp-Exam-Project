using Forum.Models;
using Forum.ViewModels.Interfaces.Forum;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Services.Interfaces.Forum
{
    public interface IForumService
    {
        void Add(IForumFormInputModel model, string category);

        IEnumerable<Models.Post> GetPostsByForum(string Id, int start);

        SubForum GetForum(string Id);

        void Edit(IForumInputModel model, string forumId);

        IForumFormInputModel GetMappedForumModel(SubForum forum);

        void Delete(SubForum forum);

        IEnumerable<SubForum> GetAllForums(ClaimsPrincipal principal);

        IEnumerable<string> GetForumPostsIds(string id);
    }
}