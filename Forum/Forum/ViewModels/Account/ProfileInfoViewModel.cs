using AutoMapper;
using Forum.MapConfiguration.Contracts;
using Forum.Models;
using System;

namespace Forum.Web.ViewModels.Account
{
    public class ProfileInfoViewModel : IHaveCustomMappings
    {
        public string Username { get; set; }

        public DateTime RegisteredOn { get; set; }

        public string Gender { get; set; }

        public string Location { get; set; }

        public int PostsCount { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ForumUser, ProfileInfoViewModel>()
                .ForMember(dest => dest.Username,
                x => x.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Location,
                x => x.MapFrom(src => src.Location))
                .ForMember(dest => dest.RegisteredOn,
                x => x.MapFrom(src => src.RegisteredOn))
                .ForMember(dest => dest.Gender,
                x => x.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.PostsCount,
                x => x.MapFrom(src => src.Posts.Count));
        }
    }
}