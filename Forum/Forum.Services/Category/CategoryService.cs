namespace Forum.Services.Category
{
    using global::Forum.Models;
    using global::Forum.Services.Category.Contracts;
    using global::Forum.Services.Db;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    public class CategoryService : ICategoryService
    {
        private readonly DbService dbService;

        public CategoryService(DbService dbService)
        {
            this.dbService = dbService;
        }

        public void AddCategory(Category model, ForumUser user)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.User = user;
            model.UserId = user.Id;

            this.dbService.DbContext.Categories.Add(model);
            this.dbService.DbContext.SaveChanges();
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

        public string[] GetCategoriesNames()
        {
            string[] categoriesNames =
                this.dbService
                .DbContext
                .Categories
                .Select(x => x.Name)
                .ToArray();

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