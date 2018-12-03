using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.Services.Interfaces.Category;
using Forum.Services.Interfaces.Db;
using Forum.ViewModels.Interfaces.Forum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Forum
{
    public class ForumInputModel : IForumInputModel, IMapFrom<SubForum>
    {
        private IDbService dbService;
        private ICategoryService categoryService;

        [Required]
        [RegularExpression(@"^[a-zA-Z_\-0-9]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-'")]
        [StringLength(50, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z _\/\-0-9!.?()&]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-', '(', ')', '&', '.', '/', '?', '!'")]
        [StringLength(500, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Description { get; set; }
        
        public string Category { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            this.dbService = (IDbService)validationContext
                     .GetService(typeof(IDbService));

            this.categoryService = (ICategoryService)validationContext
                   .GetService(typeof(ICategoryService));

            if (this.categoryService.IsCategoryValid(this.Category))
            {
                yield return ValidationResult.Success;
            }
            else
            {
                yield return new ValidationResult("Invalid category.");
            }
        }
    }
}