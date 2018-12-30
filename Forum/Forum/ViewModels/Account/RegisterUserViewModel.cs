namespace Forum.Web.ViewModels.Account
{
    using Forum.Web.Attributes.CustomValidationAttributes;
    using global::Forum.MapConfiguration.Contracts;
    using global::Forum.Models;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserViewModel : IMapTo<ForumUser>
    {
        [Required(ErrorMessage = "You must enter a username.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        [UsernameExists("{0} already exists.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter an email address.")]
        [EmailAddress]
        [EmailExists("{0} already exists.")]
        public string Email { get; set; }

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
        
        [AllowedImageExtensions()]
        public IFormFile Image { get; set; }
    }
}