using AutoMapper;
using Forum.Models;
using Forum.Models.Enums;
using Forum.Services.Interfaces.Category;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Forum;
using Forum.ViewModels.Forum;
using Forum.ViewModels.Interfaces.Forum;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Forum.Services.Forum
{

    public class ForumService : BaseService, IForumService
    {
        private readonly ICategoryService categoryService;

        public ForumService(IMapper mapper, IDbService dbService, ICategoryService categoryService)
            : base(mapper, dbService)
        {
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

        public IEnumerable<Models.Post> GetPostsByForum(string id, int start)
        {
            SubForum forum = this.GetForum(id);

            var posts =
                forum.Posts
                .Skip(start)
                .Take(5)
                .OrderBy(p => p.StartedOn)
                .ToList() ?? new List<Models.Post>();

            return posts;
        }

        public void Add(IForumFormInputModel model, string categoryId)
        {
            var forum =
                  this.mapper
                  .Map<SubForum>(model);

            Models.Category category = this.categoryService.GetCategoryById(categoryId);

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
            var forumPosts = this.dbService.DbContext.Posts.Where(p => p.ForumId == forum.Id);

            this.dbService.DbContext.RemoveRange(forumPosts);
            this.dbService.DbContext.Remove(forum);
            this.dbService.DbContext.SaveChanges();
        }

        public IEnumerable<SubForum> GetAllForums(ClaimsPrincipal principal)
        {
            if (principal.IsInRole(Common.Role.Administrator) || principal.IsInRole(Common.Role.Owner))
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

        public IEnumerable<string> GetForumPostsIds(string id)
        {
            var postsIds =
                this.GetPostsByForum(id, 0)
                .Select(p => p.Id)
                .ToList();

            return postsIds;
        }
    }
}