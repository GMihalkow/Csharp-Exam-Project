namespace Forum.Web.ViewModels.Category
{
    using global::Forum.MapConfiguration.Contracts;
    using global::Forum.Models;
    using System.ComponentModel.DataAnnotations;

    public class CategoryInputModel : IMapTo<Category>
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} name must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(CategoryType), ErrorMessage = "You must enter a valid {0} value.")]
        public CategoryType Type { get; set; }
    }
}