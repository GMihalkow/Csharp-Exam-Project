using Forum.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Interfaces.Forum
{
    public interface IForumInputModel
    {
        [Required(ErrorMessage = ErrorConstants.RequiredError)]
        [RegularExpression(ModelsConstants.NamesRegex, ErrorMessage = ErrorConstants.NamesAllowedCharactersError)]
        [StringLength(ErrorConstants.MaximumNamesLength, ErrorMessage = ErrorConstants.StringLengthErrorMessage, MinimumLength = ErrorConstants.MinimumNamesLength)]
        string Name { get; set; }

        [Required(ErrorMessage = ErrorConstants.RequiredError)]
        [RegularExpression(ModelsConstants.DescriptionsRegex, ErrorMessage = ErrorConstants.DescriptionsAllowedCharactersError)]
        [MinLength(ErrorConstants.MinimumDescriptionLength, ErrorMessage = ErrorConstants.MinimumLengthError)]
        string Description { get; set; }

        string Category { get; set; }
    }
}