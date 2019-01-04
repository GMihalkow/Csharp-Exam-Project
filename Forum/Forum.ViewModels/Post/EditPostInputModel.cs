using AutoMapper;
using Forum.MapConfiguration.Contracts;
using Forum.Services.Interfaces.Forum;
using Forum.ViewModels.Interfaces.Post;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Post
{
    public class EditPostInputModel : IEditPostInputModel, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_\-0-9]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-'")]
        [StringLength(50, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public string ForumName { get; set; }

        public IEnumerable<Models.SubForum> AllForums { get; set; }

        public string ForumId { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Models.Post, EditPostInputModel>()
                .ForMember(dest => dest.Id,
                x => x.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description,
                x => x.MapFrom(src => src.Description))
                .ForMember(dest => dest.ForumName,
                x => x.MapFrom(src => src.Forum.Name))
                .ForMember(dest => dest.Name,
                x => x.MapFrom(src => src.Name));
        }
        
    }
}