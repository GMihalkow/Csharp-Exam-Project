namespace Forum.Services.Forum
{
    using AutoMapper;
    using global::Forum.Models;
    using global::Forum.Models.Enums;
    using global::Forum.Services.Interfaces.Category;
    using global::Forum.Services.Interfaces.Db;
    using global::Forum.Services.Interfaces.Forum;
    using global::Forum.ViewModels.Forum;
    using global::Forum.ViewModels.Interfaces.Forum;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
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

        public SubForum GetForum(string id)
        {
            SubForum forum =
                this.dbService
                .DbContext
                .Forums
                .Where(f => f.Id == id)
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .ThenInclude(f => f.Author)
                .FirstOrDefault();

            return forum;
        }

        public async Task<SubForum> GetPostsByForum(string id)
        {
            SubForum forum = this.GetForum(id);

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

        public void Edit(IForumInputModel model, string forumId)
        {
            var forum = this.GetForum(forumId);
            var category = this.categoryService.GetCategoryById(model.Category);

            forum.Description = model.Description;
            forum.Name = model.Name;
            forum.Category = category;
            forum.CategoryId = category.Id;

            this.dbService.DbContext.Entry(forum).State = EntityState.Modified;
            this.dbService.DbContext.SaveChanges();
        }

        public IForumFormInputModel GetMappedForumModel(SubForum forum)
        {
            var model = this.mapper.Map<ForumInputModel>(forum);

            var names = this.categoryService.GetAllCategories().GetAwaiter().GetResult();

            var namesList =
                names
                .Select(x => new SelectListItem
                {
                    Value = x.Id,
                    Text = x.Name
                })
                .ToList();

            var forumForumModel = new ForumFormInputModel
            {
                ForumModel = model,
                Categories = namesList
            };

            return forumForumModel;
        }

        public void Delete(SubForum forum)
        {
            this.dbService.DbContext.Remove(forum);
            this.dbService.DbContext.SaveChanges();
        }

        public IEnumerable<SubForum> GetAllForums(ClaimsPrincipal principal)
        {
            if (principal.IsInRole("Administrator"))
            {
                var forums =
                    this.dbService
                    .DbContext
                    .Forums
                    .ToList();

                return forums;
            }
            else
            {
                var forums =
                    this.dbService
                    .DbContext
                    .Forums
                    .Include(f => f.Category)
                    .Where(f => f.Category.Type != CategoryType.AdminOnly)
                    .ToList();

                return forums;
            }
        }
    }
}