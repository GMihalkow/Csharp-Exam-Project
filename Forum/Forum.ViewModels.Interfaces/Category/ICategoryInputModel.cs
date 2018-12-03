using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.ViewModels.Interfaces;

namespace Forum.ViewModels.Interfaces.Category
{
    public interface ICategoryInputModel : IMapTo<global::Forum.Models.Category>
    {
        string Name { get; set; }

        CategoryType Type { get; set; }
    }
}