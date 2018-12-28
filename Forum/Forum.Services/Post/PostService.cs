namespace Forum.Services.Post.Contracts
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Interfaces.Db;
    using global::Forum.Services.Interfaces.Forum;
    using global::Forum.Services.Interfaces.Post;
    using global::Forum.Services.Interfaces.Quote;
    using global::Forum.ViewModels.Interfaces.Post;
    using global::Forum.ViewModels.Post;
    using global::Forum.ViewModels.Quote;
    using global::Forum.ViewModels.Reply;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class PostService : IPostService
    {
        private readonly IMapper mapper;
        private readonly IQuoteService quoteService;
        private readonly IDbService dbService;
        private readonly IForumService forumService;

        public PostService(IMapper mapper, IQuoteService quoteService, IDbService dbService, IForumService forumService)
        {
            this.mapper = mapper;
            this.quoteService = quoteService;
            this.dbService = dbService;
            this.forumService = forumService;
        }

        public async Task AddPost(IPostInputModel model, ForumUser user, string forumId)
        {
            var post = this.mapper.Map<Post>(model);

            var forum =
                this.forumService
                .GetForum(forumId);

            post.StartedOn = DateTime.UtcNow;
            post.Views = 0;
            post.Author = user;
            post.AuthorId = user.Id;

            await this.dbService.DbContext.Posts.AddAsync(post);
            await this.dbService.DbContext.SaveChangesAsync();
        }

        public bool DoesPostExist(string Id)
        {
            var result = this.dbService.DbContext.Posts.Any(p => p.Id == Id);
            return result;
        }

        public int Edit(IEditPostInputModel model)
        {
            var post =
                this.dbService
                .DbContext
                .Posts
                .Where(p => p.Id == model.Id)
                .FirstOrDefault();

            post.Name = model.Name;
            post.ForumId = model.ForumId;
            post.Description = model.Description;

            return this.dbService.DbContext.SaveChanges();
        }

        public IEditPostInputModel GetEditPostModel(string Id, ClaimsPrincipal principal)
        {
            var post =
                this.dbService
                .DbContext
                .Posts
                .Include(p => p.Forum)
                .Where(p => p.Id == Id)
                .FirstOrDefault();

            var model = this.mapper.Map<EditPostInputModel>(post);

            model.AllForums = this.forumService.GetAllForums(principal);

            return model;
        }

        public IEnumerable<ILatestPostViewModel> GetLatestPosts()
        {
            var latestPosts =
                this.dbService
                .DbContext
                .Posts
                .Include(p => p.Author)
                .OrderByDescending(p => p.StartedOn)
                .Take(3)
                .Select(p => this.mapper.Map<LatestPostViewModel>(p))
                .ToList();

            return latestPosts;
        }

        public IEnumerable<IPopularPostViewModel> GetPopularPosts()
        {
            var popularPosts =
                this.dbService
                .DbContext
                .Posts
                .OrderByDescending(p => p.Views)
                .Take(3)
                .Select(p => this.mapper.Map<PopularPostViewModel>(p))
                .ToList();

            return popularPosts;
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
                .ThenInclude(p => p.Author)
                .ThenInclude(p => p.Posts)
                .Include(p => p.Replies)
                .ThenInclude(p => p.Quotes)
                .Include(p => p.Replies)
                .ThenInclude(p => p.Quotes)
                .ThenInclude(p => p.Author)
                .ThenInclude(p => p.Posts)
                .FirstOrDefault(p => p.Id == id);

            var replies =
                post
                .Replies
                .Select(r => this.mapper.Map<ReplyViewModel>(r));

            PostViewModel viewModel = this.mapper.Map<PostViewModel>(post);

            return viewModel;
        }

        public string ParseDescription(string description)
        {
            string[] inputArray =
                description
                .Split(Environment.NewLine)
                .ToArray();

            var sb = new StringBuilder();

            string pattern = @"(\[(\w+)\])(.*?)(\[\/\2\])";
            Regex tagsRegex = new Regex(pattern);

            for (int index = 0; index < inputArray.Length; index++)
            {
                var match = tagsRegex.Match(inputArray[index]);

                if (match.Success)
                {
                    while (match.Success)
                    {
                        match = tagsRegex.Match(inputArray[index]);

                        int lineLength = inputArray[index].Length;
                        if (lineLength < 0)
                        {
                            lineLength = 0;
                        }

                        //getting the text before the match
                        var stringBeggining = inputArray[index].Substring(0, match.Index);

                        //the match
                        //opening tag
                        var openingTag = match.Groups[1].Value;
                        openingTag = openingTag.Replace(']', '>');
                        openingTag = openingTag.Replace('[', '<');

                        //middle text
                        var text = match.Groups[3].Value;

                        //closing tag
                        var closingTag = match.Groups[4].Value;
                        closingTag = closingTag.Replace(']', '>');
                        closingTag = closingTag.Replace('[', '<');

                        int lastMatchIndex = (match.Length + match.Index);
                        if (lastMatchIndex < 0)
                        {
                            lastMatchIndex = 0;
                        }
                        //getting the text after the match
                        var restOfString = inputArray[index].Substring(lastMatchIndex, lineLength - lastMatchIndex);

                        inputArray[index] = stringBeggining + openingTag + text + closingTag + restOfString;
                    }
                    sb.AppendLine(inputArray[index]);
                }
                else
                {
                    sb.AppendLine(inputArray[index]);
                }
            }

            return sb.ToString().TrimEnd();
        }

        public int ViewPost(string id)
        {
            var post =
                this.dbService
                .DbContext
                .Posts
                .Where(p => p.Id == id)
                .FirstOrDefault();

            post.Views++;

            this.dbService.DbContext.SaveChanges();

            return post.Views;
        }
    }
}