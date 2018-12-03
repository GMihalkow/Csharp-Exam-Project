using Forum.MapConfiguration.Contracts;

namespace Forum.Web.ViewModels.Forum
{
    using AutoMapper;
    using global::Forum.Models;

    //TODO: Add validation
    public class ForumFormInputModel : IHaveCustomMappings
    {
        public ForumInputModel ForumModel { get; set; }

        public string[] Categories { get; set; }

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