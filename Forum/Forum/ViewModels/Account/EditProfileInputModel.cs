namespace Forum.Web.ViewModels.Account
{
    using AutoMapper;
    using Forum.MapConfiguration.Contracts;
    using Forum.Web.Attributes.CustomValidationAttributes;
    using System.ComponentModel.DataAnnotations;

    public class EditProfileInputModel : IHaveCustomMappings
    {
        [Required(ErrorMessage = "You must enter a username.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        [UsernameExists("{0} already exists.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter a country name.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 2)]
        public string Location { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You must enter a password.")]
        [Compare("ConfirmPassword", ErrorMessage = "Passwords do not match.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You must confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must select a gender.")]
        public string Gender { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Models.ForumUser, EditProfileInputModel>()
                .ForMember(dest => dest.Username,
                    x => x.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Gender,
                    x => x.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.Location,
                    x => x.MapFrom(src => src.Location));
        }
    }
}