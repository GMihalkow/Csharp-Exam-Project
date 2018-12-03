namespace Forum.Services.Forum
{
    using global::Forum.Models;
    using global::Forum.Services.Category.Contracts;
    using global::Forum.Services.Db;
    using global::Forum.Services.Forum.Contracts;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ForumService : IForumService
    {
        private readonly DbService dbService;
        private readonly ICategoryService categoryService;

        public ForumService(DbService dbService, ICategoryService categoryService)
        {
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

        public void Add(SubForum model, string categoryId)
        {
            Category category = this.categoryService.GetCategoryById(categoryId);

            model.CreatedOn = DateTime.UtcNow;
            model.CategoryId = category.Id;
            model.Category = category;

            this.dbService.DbContext.Forums.Add(model);
            this.dbService.DbContext.SaveChanges();
        }

        public async Task<SubForum> GetPostsByForum(string id)
        {
            SubForum forum = await this.GetForum(id);

            return forum;
        }
    }
}