namespace Forum.Services.Category.Contracts
{
    using global::Forum.Models;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Category GetCategory(string name);

        Task<string[]> GetCategoriesNames();

        Task<int> AddCategory(Category model, ForumUser user);

        Category[] GetAllCategories();

        Category[] GetUsersCategories();
    }
}