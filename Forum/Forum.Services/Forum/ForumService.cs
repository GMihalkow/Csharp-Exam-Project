namespace Forum.Services.Forum
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Category;
    using global::Forum.Services.Db;
    using global::Forum.Services.Interfaces.Category;
    using global::Forum.Services.Interfaces.Db;
    using global::Forum.Services.Interfaces.Forum;
    using global::Forum.ViewModels.Forum;
    using global::Forum.ViewModels.Interfaces.Forum;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class ForumService : IForumService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;
        private readonly ICategoryService categoryService;

        public ForumService(IMapper mapper, IDbService dbService, ICategoryService categoryService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
            this.categoryService = categoryService;
        }

        public async Task<SubForum> GetForum(string id)
        {
            SubForum forum =
                await 
                this.dbService
                .DbContext
                .Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .ThenInclude(f => f.Author)
                .FirstOrDefaultAsync(f => f.Id == id);

            return forum;
        }
        
        public async Task<SubForum> GetPostsByForum(string id)
        {
            SubForum forum = await this.GetForum(id);

            return forum;
        }

        public void Add(IForumFormInputModel model, string categoryId)
        {
            var forum =
                  this.mapper
                  .Map<SubForum>(model);

            Category category = this.categoryService.GetCategoryById(categoryId);

            forum.CreatedOn = DateTime.UtcNow;
            forum.CategoryId = categoryId;
            forum.Category = category;

            this.dbService.DbContext.Forums.Add(forum);
            this.dbService.DbContext.SaveChanges();
        }
    }
}