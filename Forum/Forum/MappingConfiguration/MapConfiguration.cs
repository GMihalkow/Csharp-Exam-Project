namespace Forum.Web.MappingConfiguration
{
    using AutoMapper;
    using Forum.Models;
    using Forum.Web.ViewModels.Account;
    using Forum.Web.ViewModels.Category;
    using Forum.Web.ViewModels.Forum;
    using Forum.Web.ViewModels.Post;

    public class MapConfiguration : Profile
    {
        public MapConfiguration()
        {
            this.CreateMap<LoginUserInputModel, ForumUser>();

            this.CreateMap<RegisterUserViewModel, ForumUser>();

            this.CreateMap<CategoryInputModel, Category>();

            this.CreateMap<ForumFormInputModel, SubForum>()
                .ForMember(f => f.Description,
                    x => x.MapFrom(src => src.ForumModel.Description))
                .ForMember(f => f.Name,
                    x => x.MapFrom(src => src.ForumModel.Name));

            this.CreateMap<ForumPostsInputModel, SubForum>();

            this.CreateMap<PostInputModel, Post>()
                .ForMember(p => p.Name,
                    x => x.MapFrom(src => src.Title));
        }
    }
}