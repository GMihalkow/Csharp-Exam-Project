namespace Forum.Services.Category
{
    using global::Forum.Models;
    using global::Forum.Services.Category.Contracts;
    using global::Forum.Services.Db;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly DbService dbService;

        public CategoryService(DbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<int> AddCategory(Category model, ForumUser user)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.User = user;
            model.UserId = user.Id;

            await this.dbService.DbContext.Categories.AddAsync(model);
            return await this.dbService.DbContext.SaveChangesAsync();
        }

        public Category[] GetAllCategories()
        {
            var categories = 
                this.dbService
                .DbContext
                .Categories
                .Include(c => c.Forums)
                .ToArray();

            return categories;
        }

        public async Task<string[]> GetCategoriesNames()
        {
            string[] categoriesNames =
                await this.dbService
                .DbContext
                .Categories
                .Select(x => x.Name)
                .ToArrayAsync();

            return categoriesNames;
        }

        public Category GetCategory(string name)
        {
            Category category = 
                this.dbService
                .DbContext
                .Categories
                .FirstOrDefault(c => c.Name == name);

            return category;
        }

        public Category[] GetUsersCategories()
        {
            var categories =
                this.dbService
                .DbContext
                .Categories
                .Where(c => (int)c.Type != 2)
                .Include(c => c.Forums)
                .ToArray();

            return categories;
        }
    }
}