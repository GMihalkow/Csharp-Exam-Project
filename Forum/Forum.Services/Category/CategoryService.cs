namespace Forum.Services.Category
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Services.Db;
    using global::Forum.Services.Interfaces.Category;
    using global::Forum.Services.Interfaces.Db;
    using global::Forum.ViewModels.Category;
    using global::Forum.ViewModels.Interfaces.Category;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;

        public CategoryService(IMapper mapper,  IDbService dbService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
        }
        
        public async Task<int> AddCategory(ICategoryInputModel model, ForumUser user)
        {
            var category =
                this.mapper
                .Map<CategoryInputModel, Category>(model as CategoryInputModel);

            category.CreatedOn = DateTime.UtcNow;
            category.User = user;
            category.UserId = user.Id;

            await this.dbService.DbContext.Categories.AddAsync(category);
            return await this.dbService.DbContext.SaveChangesAsync();
        }

        public async Task<Category[]> GetAllCategories()
        {
            var categories =
                this.dbService
                .DbContext
                .Categories
                .Include(c => c.Forums)
                .ToArrayAsync();

            return categories.GetAwaiter().GetResult();
        }

        public Category GetCategoryById(string Id)
        {
            var category =
                this.dbService
                .DbContext
                .Categories
                .FirstOrDefault(c => c.Id == Id);

            return category;
        }

        public Category GetCategoryByName(string name)
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
        
        public bool IsCategoryValid(string id)
        {
            var result =
                this.dbService
                .DbContext
                .Categories
                .Any(c => c.Id == id);

            return result;
        }
    }
}