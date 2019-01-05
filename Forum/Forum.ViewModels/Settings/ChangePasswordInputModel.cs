using Forum.Services.Interfaces.Account;
using Forum.Services.Interfaces.Settings;
using Forum.ViewModels.Interfaces.Settings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Settings
{
    public class ChangePasswordInputModel : IChangePasswordInputModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You must enter a password.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You must enter a password.")]
        [StringLength(50, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string NewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var settingsService = (ISettingsService)validationContext
                   .GetService(typeof(ISettingsService));

            var accountService = (IAccountService)validationContext
                  .GetService(typeof(IAccountService));

            var user = accountService.GetUserByName(this.Username);

            var model = validationContext.ObjectInstance as ChangePasswordInputModel;

            var result = settingsService.CheckPassword(user, model.OldPassword);
            if (!result)
            {
                yield return new ValidationResult("Incorrect password");
            }
            else
            {
                yield return ValidationResult.Success;
            }
        }
    }
}