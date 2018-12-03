using Forum.MapConfiguration.Contracts;
using AutoMapper;
using Forum.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Forum.Web.ViewModels.Forum
{
    public class ForumFormInputModel : IHaveCustomMappings
    {
        public ForumInputModel ForumModel { get; set; }

        public SelectListItem[] Categories { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ForumFormInputModel, SubForum>()
                   .ForMember(f => f.Description,
                       x => x.MapFrom(src => src.ForumModel.Description))
                   .ForMember(f => f.Name,
                       x => x.MapFrom(src => src.ForumModel.Name));
        }
    }
}