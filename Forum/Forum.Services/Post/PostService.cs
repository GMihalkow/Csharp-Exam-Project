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
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

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

                        var stringBeggining = inputArray[index].Substring(0, match.Index);

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
    }
}