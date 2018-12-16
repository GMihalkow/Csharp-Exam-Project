using AutoMapper;
using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.Services.Interfaces.Category;
using Forum.ViewModels.Interfaces.Forum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Forum
{
    public class ForumInputModel : IForumInputModel, IValidatableObject,  IHaveCustomMappings, IMapTo<SubForum>
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z_\-0-9]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-'")]
        [StringLength(50, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z _\/\-0-9!.?()&]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-', '(', ')', '&', '.', '/', '?', '!'")]
        [StringLength(500, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Description { get; set; }
        
        public string Category { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            var categoryService = (ICategoryService)validationContext
                   .GetService(typeof(ICategoryService));

            if (categoryService.IsCategoryValid(this.Category))
            {
                yield return ValidationResult.Success;
            }
            else
            {
                yield return new ValidationResult("Invalid category.");
            }
        }
        
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<SubForum, ForumInputModel>()
                .ForMember(fm => fm.Category,
                x => x.MapFrom(src => src.Category.Name))
                .ForMember(fm => fm.Name,
                x => x.MapFrom(src => src.Name))
                .ForMember(fm => fm.Description,
                x => x.MapFrom(src => src.Description));

            //configuration.CreateMap<ForumInputModel, SubForum>()
            //    .ForMember(sf => sf.Description,
            //    x => x.MapFrom(src => src.Description))
            //    .ForMember(sf => sf.Name,
            //    x => x.MapFrom(src => src.Name));
        }
    }
}