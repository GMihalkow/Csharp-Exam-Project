using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Attributes.CustomValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class AllowedImageExtensions : ValidationAttribute
    {
        public AllowedImageExtensions()
        {
        }

        public AllowedImageExtensions(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var image = value as IFormFile;
            if (image == null)
            {
                return ValidationResult.Success;
            }

            var manageAccountService = (Areas.Profile.Services.Account.Contracts.IProfileService)validationContext
                   .GetService(typeof(Areas.Profile.Services.Account.Contracts.IProfileService));

            var result = manageAccountService.IsImageExtensionValid(image.FileName);

            if (result)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Allowed image extansions are .jpeg, .png, .jpg and .bmp");
            }
        }
    }
}