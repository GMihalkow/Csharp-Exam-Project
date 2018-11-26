namespace Forum.Web.Services
{
    using Forum.Models;
    using Forum.Web.Services.Contracts;
    using Forum.Web.ViewModels.Forum;
    using System;
    using System.Collections.Generic;
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

        public ICollection<Post> GetPostsByForum(string id)
        {
            var posts =
                this.dbService
                .DbContext
                .Posts
                .Where(p => p.Forum.Id == id)
                .ToArray();

            return posts;
        }
    }
}