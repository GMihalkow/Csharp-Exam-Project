using Forum.Models;
using Forum.Services.Account.Contracts;
using Forum.Services.Category.Contracts;
using Forum.Services.Forum.Contracts;
using Forum.Web.ViewModels.Forum;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Forum.Web.Controllers.Forum
{
    [Authorize("Admin")]
    public class ForumController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;
        private readonly IForumService forumService;

        public ForumController(IMapper mapper, IAccountService accountService, ICategoryService categoryService, IForumService forumService)
            : base(accountService)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
            this.forumService = forumService;
        }

        public IActionResult Create()
        {
            var names = this.categoryService.GetAllCategories();

            var namesList = 
                names
                //.GetAwaiter().GetResult()
                .Select(x => new SelectListItem
                {
                    Value = x.Id,
                    Text = $"{x.Name} ({x.ForumsCount})"
                })
                .ToArray();

            ForumFormInputModel model = new ForumFormInputModel
            {
                Categories = namesList
            };
            
            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(ForumFormInputModel model)
        {
            if(ModelState.IsValid)
            {
                var forum =
                    this.mapper
                    .Map<SubForum>(model);

                this.forumService.Add(forum, model.ForumModel.Category);

                return this.Redirect("/");
            }
            else
            {
                var names = this.categoryService.GetAllCategories();
                
                var namesList =
                    names
                    //.GetAwaiter().GetResult()
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id,
                        Text = $"{x.Name} ({x.ForumsCount})"
                    })
                    .ToArray();

                ForumFormInputModel viewModel = new ForumFormInputModel
                {
                    ForumModel = model.ForumModel,
                    Categories = namesList
                };

                return this.View(viewModel);
            }
        }

        [AllowAnonymous]
        public IActionResult Posts(string id)
        {
            var forum = this.forumService.GetPostsByForum(id).GetAwaiter().GetResult();

            var model = new ForumPostsInputModel
            {
                Forum = forum,
                Posts = forum.Posts
            };
            
            return this.View(model);
        }
    }
}