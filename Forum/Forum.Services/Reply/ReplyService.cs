using System;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Models;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Interfaces.Reply;

namespace Forum.Services.Reply
{
    public class ReplyService : IReplyService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;
        private readonly IPostService postService;

        public ReplyService(IMapper mapper, IDbService dbService, IPostService postService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
            this.postService = postService;
        }

        public async Task<int> Add(IReplyInputModel model, ForumUser user)
        {
            var reply = this.mapper.Map<Models.Reply>(model);

            reply.Description = this.postService.ParseDescription(model.Description);
            reply.Author = user;
            reply.AuthorId = user.Id;
            reply.RepliedOn = DateTime.UtcNow;

            await this.dbService.DbContext.Replies.AddAsync(reply);
            return await this.dbService.DbContext.SaveChangesAsync();
        }
    }
}