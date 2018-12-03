using AutoMapper;
using Forum.Attributes;
using Forum.MapConfiguration.Contracts;
using Forum.Models;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.ViewModels.Account
{
    [UserExists("Invalid login attempt.")]
    public class LoginUserInputModel : IMapTo<ForumUser>
    {
        [Required(ErrorMessage = "You must enter a username.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter a password.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //public void CreateMappings(IMapperConfigurationExpression configuration)
        //{
        //    configuration.CreateMap<LoginUserInputModel, ForumUser>()
        //        .ForMember(dest => dest.UserName,
        //        x => x.MapFrom(src => src.Username));
        //}
    }
}