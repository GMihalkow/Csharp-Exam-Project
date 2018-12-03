namespace Forum.Services.Interfaces.Category
{
    using global::Forum.Models;
    using global::Forum.ViewModels.Interfaces.Category;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Category GetCategoryByName(string name);

        Category GetCategoryById(string Id);

        Task<int> AddCategory(ICategoryInputModel model, ForumUser user);

        Task<Category[]> GetAllCategories();

        Category[] GetUsersCategories();

        bool IsCategoryValid(string name);
    }
}