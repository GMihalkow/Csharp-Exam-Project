using Forum.Services.Interfaces.Account;
using Forum.Services.Interfaces.Settings;
using Forum.ViewModels.Common;
using Forum.ViewModels.Interfaces.Settings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Settings
{
    public class ChangePasswordInputModel : IChangePasswordInputModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = ErrorConstants.RequiredError)]
        [StringLength(ErrorConstants.MaximumPasswordsLength, ErrorMessage = ErrorConstants.StringLengthErrorMessage, MinimumLength = ErrorConstants.MinimumPasswordsLength)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = ErrorConstants.RequiredError)]
        [StringLength(ErrorConstants.MaximumPasswordsLength, ErrorMessage = ErrorConstants.StringLengthErrorMessage, MinimumLength = ErrorConstants.MinimumPasswordsLength)]
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
                yield return new ValidationResult(ErrorConstants.IncorrectPasswordError);
            }
            else
            {
                yield return ValidationResult.Success;
            }
        }
    }
}