namespace Forum.Services.Post.Contracts
{
    using global::Forum.Models;
    using global::Forum.Services.Db;
    using global::Forum.Services.Forum.Contracts;
    using System;

    public class PostService : IPostService
    {
        private readonly DbService dbService;
        private readonly IForumService forumService;

        public PostService(DbService dbService, IForumService forumService)
        {
            this.dbService = dbService;
            this.forumService = forumService;
        }

        public void AddPost(Post model, ForumUser user, string forumId)
        {
            var forum = 
                this.forumService
                .GetForum(forumId);

            model.StartedOn = DateTime.UtcNow;
            model.Views = 0;
            model.Author = user;
            model.AuthorId = user.Id;

            this.dbService.DbContext.Posts.Add(model);
            this.dbService.DbContext.SaveChanges();
        }
    }
}