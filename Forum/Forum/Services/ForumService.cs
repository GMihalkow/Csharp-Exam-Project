namespace Forum.Web.Services
{
    using Forum.Models;
    using Forum.Web.Services.Contracts;
    using Forum.Web.ViewModels.Forum;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    public class ForumService : IForumService
    {
        private readonly DbService dbService;
        private readonly ICategoryService categoryService;

        public ForumService(DbService dbService, ICategoryService categoryService)
        {
            this.dbService = dbService;
            this.categoryService = categoryService;
        }

        public SubForum GetForum(string id)
        {
            SubForum forum = this.dbService
                .DbContext
                .Forums
                .Include(f => f.Category)
                .FirstOrDefault(f => f.Id == id);

            return forum;
        }

        public void Add(ForumFormInputModel model)
        {
            Category category = this.categoryService.GetCategory(model.ForumModel.Category);

            SubForum subForum = new SubForum
            {
                Name = model.ForumModel.Name,
                Category = category,
                CategoryId = category.Id,
                CreatedOn = DateTime.UtcNow,
                Description = model.ForumModel.Description
            };

            this.dbService.DbContext.Forums.Add(subForum);
            this.dbService.DbContext.SaveChanges();
        }

        public ForumPostsInputModel GetPostsByForum(string id)
        {
            SubForum forum = this.GetForum(id);

            ForumPostsInputModel model = new ForumPostsInputModel
            {
                Forum = forum,
                Posts =
                this.dbService
                .DbContext
                .Posts
                .Include(p => p.Forum)
                .Include(p => p.Author)
                .Where(p => p.Forum.Id == id)
                .ToArray()
            };

            return model;
        }
    }
}