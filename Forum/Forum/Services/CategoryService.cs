namespace Forum.Web.Services
{
    using Forum.Models;
    using Forum.Web.Services.Contracts;
    using Forum.Web.ViewModels.Category;
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

        public void AddCategory(CategoryInputModel model, ForumUser user)
        {
            Category category = new Category
            {
                Name = model.Name,
                CreatedOn = DateTime.UtcNow,
                Type = model.Type,
                User = user,
                UserId = user.Id
            };

            this.dbService.DbContext.Categories.Add(category);
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