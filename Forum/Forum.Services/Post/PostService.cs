namespace Forum.Services.Post.Contracts
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Db;
    using global::Forum.Services.Interfaces.Db;
    using global::Forum.Services.Interfaces.Forum;
    using global::Forum.Services.Interfaces.Post;
    using global::Forum.ViewModels.Interfaces.Post;
    using global::Forum.ViewModels.Post;
    using System;

    public class PostService : IPostService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;
        private readonly IForumService forumService;

        public PostService(IMapper mapper, IDbService dbService, IForumService forumService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
            this.forumService = forumService;
        }

        public void AddPost(IPostInputModel model, ForumUser user, string forumId)
        {
            var post = this.mapper.Map<Post>(model);

            var forum = 
                this.forumService
                .GetForum(forumId);

            post.StartedOn = DateTime.UtcNow;
            post.Views = 0;
            post.Author = user;
            post.AuthorId = user.Id;

            this.dbService.DbContext.Posts.Add(post);
            this.dbService.DbContext.SaveChanges();
        }
    }
}