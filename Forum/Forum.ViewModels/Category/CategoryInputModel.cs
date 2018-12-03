namespace Forum.ViewModels.Category
{
    using global::Forum.MapConfiguration.Contracts;
    using System.ComponentModel.DataAnnotations;
    using global::Forum.Models;
    using global::Forum.ViewModels.Interfaces;
    using global::Forum.ViewModels.Interfaces.Category;

    public class CategoryInputModel : ICategoryInputModel, IMapTo<Category>
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} name must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(CategoryType), ErrorMessage = "You must enter a valid {0} value.")]
        public CategoryType Type { get; set; }
    }
}