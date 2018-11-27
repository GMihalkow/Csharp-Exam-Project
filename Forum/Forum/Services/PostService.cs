namespace Forum.Web.Services
{
    using Forum.Models;
    using Forum.Web.Services.Contracts;
    using Forum.Web.ViewModels.Post;
    using System;

    public class PostService : IPostService
    {
        private readonly DbService dbService;

        public PostService(DbService dbService)
        {
            this.dbService = dbService;
        }

        public void AddPost(PostInputModel model, ForumUser user)
        {
            Post post = new Post
            {
                Name = model.Title,
                Description = model.Description,
                StartedOn = DateTime.UtcNow,
                Views = 0,
                Author = user,
                AuthorId = user.Id,
                ForumId = model.ForumId
            };

            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();
        }
    }
}