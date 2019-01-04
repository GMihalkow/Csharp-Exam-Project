using Forum.Web.Services.Account.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Areas.Settings.ViewModels.Settings
{
    public class DeleteUserInputModel : IValidatableObject
    {
        [Required(ErrorMessage = "You must enter a username.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter a password.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var accountService = (IAccountService)validationContext
                  .GetService(typeof(IAccountService));

            var model = validationContext.ObjectInstance as DeleteUserInputModel;

            var result = accountService.UserWithPasswordExists(model.Username, model.Password).GetAwaiter().GetResult();
            if (!result)
            {
                yield return new ValidationResult("Error. User not found.");
            }
            else
            {
                yield return ValidationResult.Success;
            }
        }
    }
}