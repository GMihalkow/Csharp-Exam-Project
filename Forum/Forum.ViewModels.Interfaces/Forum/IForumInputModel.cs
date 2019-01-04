using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Interfaces.Forum
{
    public interface IForumInputModel : IValidatableObject
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z_\-0-9]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-'")]
        [StringLength(50, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z _\/\-0-9!.?()&]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-', '(', ')', '&', '.', '/', '?', '!'")]
        [StringLength(500, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        string Description { get; set; }

        string Category { get; set; }
    }
}