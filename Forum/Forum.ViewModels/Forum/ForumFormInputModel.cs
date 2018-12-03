using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Forum.ViewModels.Interfaces.Forum;
using Forum.Models;

namespace Forum.ViewModels.Forum
{
    public class ForumFormInputModel : IForumFormInputModel
    {
        public ForumFormInputModel()
        {
            this.ForumModel = new ForumInputModel();
        }

        public IForumInputModel ForumModel { get; set; }

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