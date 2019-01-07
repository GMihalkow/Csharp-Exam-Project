using AutoMapper;
using Forum.Data;
using Forum.MapConfiguration;
using Forum.Models;
using Forum.Models.Enums;
using Forum.Services.Category;
using Forum.Services.Common;
using Forum.Services.Common.Comparers;
using Forum.Services.Db;
using Forum.Services.Interfaces.Category;
using Forum.ViewModels.Account;
using Forum.ViewModels.Category;
using Forum.ViewModels.Forum;
using Forum.ViewModels.Message;
using Forum.ViewModels.Post;
using Forum.ViewModels.Profile;
using Forum.ViewModels.Quote;
using Forum.ViewModels.Reply;
using Forum.ViewModels.Report;
using Forum.ViewModels.Role;
using Forum.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Forum.Services.UnitTests.Category
{
    public class CategoryServiceTests
    {
        private readonly DbContextOptionsBuilder<ForumDbContext> options;

        private readonly ForumDbContext dbContext;

        private readonly DbService dbService;

        private readonly IMapper mapper;

        private readonly CategoryService categoryService;

        public CategoryServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(databaseName: TestsConstants.InMemoryDbName);

            this.dbContext = new ForumDbContext(this.options.Options);

            this.dbService = new DbService(this.dbContext);

            this.mapper = AutoMapperConfig.RegisterMappings(
               typeof(LoginUserInputModel).Assembly,
               typeof(EditPostInputModel).Assembly,
               typeof(RegisterUserViewModel).Assembly,
               typeof(CategoryInputModel).Assembly,
               typeof(UserJsonViewModel).Assembly,
               typeof(ForumFormInputModel).Assembly,
               typeof(ForumInputModel).Assembly,
               typeof(RecentConversationViewModel).Assembly,
               typeof(ForumPostsInputModel).Assembly,
               typeof(PostInputModel).Assembly,
               typeof(LatestPostViewModel).Assembly,
               typeof(ProfileInfoViewModel).Assembly,
               typeof(PopularPostViewModel).Assembly,
               typeof(ReplyInputModel).Assembly,
               typeof(PostViewModel).Assembly,
               typeof(ReplyViewModel).Assembly,
               typeof(EditProfileInputModel).Assembly,
               typeof(SendMessageInputModel).Assembly,
               typeof(QuoteInputModel).Assembly,
               typeof(PostReportInputModel).Assembly,
               typeof(ReplyReportInputModel).Assembly,
               typeof(UserRoleViewModel).Assembly,
               typeof(ChatMessageViewModel).Assembly,
               typeof(QuoteReportInputModel).Assembly)
               .CreateMapper();

            this.categoryService = new CategoryService(this.mapper, this.dbService);
        }

        private void TruncateCategoriesTable()
        {
            var categories = this.dbService.DbContext.Categories.ToList();
            this.dbService.DbContext.Categories.RemoveRange(categories);

            this.dbService.DbContext.SaveChanges();
        }
        private void TruncateUsersable()
        {
            var categories = this.dbService.DbContext.Users.ToList();
            this.dbService.DbContext.Users.RemoveRange(categories);

            this.dbService.DbContext.SaveChanges();
        }
        [Fact]
        public void GetCategoryByName_returns_entity_when_correct()
        {
            this.TruncateCategoriesTable();

            var categoryId = Guid.NewGuid().ToString();

            var category = new Models.Category { Id = categoryId, Name = TestsConstants.ValidCategoryName };
            this.dbService.DbContext.Categories.Add(category);

            this.dbService.DbContext.SaveChanges();

            var expectedName = new Models.Category { Id = categoryId, Name = TestsConstants.ValidCategoryName }.Name;
            var actualName = this.categoryService.GetCategoryByName(TestsConstants.ValidCategoryName).Name;

            Assert.Equal(expectedName, actualName);
        }

        [Fact]
        public void GetCategoryByName_returns_null_when_correct()
        {
            this.TruncateCategoriesTable();

            Assert.Null(this.categoryService.GetCategoryByName(TestsConstants.ValidCategoryName));
        }

        [Fact]
        public void GetCategoryById_returns_entity_when_correct()
        {
            this.TruncateCategoriesTable();

            var categoryId = Guid.NewGuid().ToString();

            var category = new Models.Category { Id = categoryId, Name = TestsConstants.ValidCategoryName };
            this.dbService.DbContext.Categories.Add(category);

            this.dbService.DbContext.SaveChanges();

            var expectedId = new Models.Category { Id = categoryId, Name = TestsConstants.ValidCategoryName }.Id;
            var actualId = this.categoryService.GetCategoryByName(TestsConstants.ValidCategoryName).Id;

            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void GetCategoryById_returns_null_when_correct()
        {
            this.TruncateCategoriesTable();

            Assert.Null(this.categoryService.GetCategoryByName(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void AddCategory_returns_two_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateUsersable();

            var inputModel = new CategoryInputModel { Name = TestsConstants.ValidCategoryName, Type = CategoryType.Public };

            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };

            var result = this.categoryService.AddCategory(inputModel, user).GetAwaiter().GetResult();

            Assert.Equal(2, result);
        }

        [Fact]
        public void GetAllCategories_returns_entities_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateUsersable();

            var testCategory = new Models.Category { Id = TestsConstants.TestId, Name = TestsConstants.ValidCategoryName };

            var testCategoryTwo = new Models.Category { Id = TestsConstants.TestId1, Name = TestsConstants.ValidCategoryName1 };
            
            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };

            this.dbService.DbContext.Categories.Add(testCategoryTwo);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Categories.Add(testCategory);
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<Models.Category> { testCategory, testCategoryTwo };

            Assert.Equal(expectedList, this.categoryService.GetAllCategories());
        }

        [Fact]
        public void GetAllCategories_returns_empty_list_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateUsersable();

            var expectedList = new List<Models.Category>();

            Assert.Equal(expectedList, this.categoryService.GetAllCategories());
        }

        [Fact]
        public void GetPublicCategories_returns_entities_when_correct()
        {
            this.TruncateCategoriesTable();
            this.TruncateUsersable();

            var testCategory = new Models.Category { Id = TestsConstants.TestId, Name = TestsConstants.ValidCategoryName, Type = CategoryType.AdminOnly };

            var testCategoryTwo = new Models.Category { Id = TestsConstants.TestId1, Name = TestsConstants.ValidCategoryName1, Type = CategoryType.Public};

            var user = new ForumUser { Id = TestsConstants.TestId, UserName = TestsConstants.TestUsername1 };

            this.dbService.DbContext.Categories.Add(testCategoryTwo);
            this.dbService.DbContext.SaveChanges();

            this.dbService.DbContext.Categories.Add(testCategory);
            this.dbService.DbContext.SaveChanges();

            var expectedList = new List<Models.Category> { testCategoryTwo };

            Assert.Equal(expectedList, this.categoryService.GetPublicCategories());
        }

        [Fact]
        public void IsCategoryValid_returns_true_when_correct()
        {
            this.TruncateCategoriesTable();

            var categoryId = Guid.NewGuid().ToString();

            var category = new Models.Category { Id = categoryId, Name = TestsConstants.ValidCategoryName };
            this.dbService.DbContext.Categories.Add(category);

            this.dbService.DbContext.SaveChanges();
            
            var result = this.categoryService.IsCategoryValid(categoryId);

            Assert.True(result == true);
        }

        [Fact]
        public void IsCategoryValid_returns_false_when_incorrect()
        {
            this.TruncateCategoriesTable();

            var categoryId = Guid.NewGuid().ToString();
            
            var result = this.categoryService.IsCategoryValid(categoryId);

            Assert.True(result == false);
        }
    }
}