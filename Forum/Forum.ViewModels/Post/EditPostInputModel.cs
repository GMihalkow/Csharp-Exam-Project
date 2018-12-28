using AutoMapper;
using Forum.MapConfiguration.Contracts;
using Forum.ViewModels.Interfaces.Post;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Forum.ViewModels.Post
{
    public class EditPostInputModel : IEditPostInputModel, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

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