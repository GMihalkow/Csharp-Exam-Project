namespace Forum.Services.Post.Contracts
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Interfaces.Db;
    using global::Forum.Services.Interfaces.Forum;
    using global::Forum.Services.Interfaces.Post;
    using global::Forum.ViewModels.Interfaces.Post;
    using global::Forum.ViewModels.Post;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

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

        public async void AddPost(IPostInputModel model, ForumUser user, string forumId)
        {
            var post = this.mapper.Map<Post>(model);

            var forum =
                this.forumService
                .GetForum(forumId);

            post.StartedOn = DateTime.UtcNow;
            post.Views = 0;
            post.Author = user;
            post.AuthorId = user.Id;

            this.dbService.DbContext.Posts.AddAsync(post).GetAwaiter().GetResult();
            this.dbService.DbContext.SaveChangesAsync().GetAwaiter().GetResult();
        }

        public IPostViewModel GetPost(string id)
        {
            Post post =
                this.dbService
                .DbContext
                .Posts
                .Include(p => p.Author)
                .ThenInclude(p => p.Posts)
                .Include(p => p.Replies)
                .FirstOrDefault(p => p.Id == id);

            PostViewModel viewModel = this.mapper.Map<PostViewModel>(post);

            return viewModel;
        }

        public string ParseDescription(string description)
        {
            string[] inputArray =
                description
                .Split(Environment.NewLine)
                .ToArray();

            string pattern = @"(\[(\w+)\])(.*?)(\[\/\2\])";
            Regex tagsRegex = new Regex(pattern);

            for (int index = 0; index < inputArray.Length; index++)
            {
                inputArray[index] = inputArray[index].Replace(']', '>');
                inputArray[index] = inputArray[index].Replace('[', '<');
            }

            string result = string.Join(Environment.NewLine, inputArray);

            return result;
        }
    }
}