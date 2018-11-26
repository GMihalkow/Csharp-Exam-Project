namespace Forum.Web.Services.Contracts
{
    using Forum.Models;
    using Forum.Web.ViewModels.Category;

    public interface ICategoryService
    {
        void AddCategory(CategoryInputModel model, ForumUser user);

        Category[] GetAllCategories();

        Category[] GetUsersCategories();
    }
}