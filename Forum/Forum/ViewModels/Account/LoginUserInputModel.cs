namespace Forum.Web.ViewModels.Account
{
    using global::Forum.Web.Attributes;
    using System.ComponentModel.DataAnnotations;

    [UserExists("Invalid login attempt.")]
    public class LoginUserInputModel
    {
        [Required(ErrorMessage = "You must enter a username.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter a password.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}