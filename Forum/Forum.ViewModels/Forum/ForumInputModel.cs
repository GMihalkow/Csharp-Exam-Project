using AutoMapper;
using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.Services.Interfaces.Category;
using Forum.ViewModels.Interfaces.Forum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Forum
{
    public class ForumInputModel : IForumInputModel, IHaveCustomMappings, IMapTo<SubForum>
    {
        public string Name { get; set; }
        
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
                yield return new ValidationResult("Error. Invalid category.");
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
        }
    }
}