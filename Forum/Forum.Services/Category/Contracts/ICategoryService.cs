namespace Forum.Services.Category.Contracts
{
    using global::Forum.Models;

    public interface ICategoryService
    {
        Category GetCategory(string name);

        string[] GetCategoriesNames();

        void AddCategory(Category model, ForumUser user);

        Category[] GetAllCategories();

        Category[] GetUsersCategories();
    }
}