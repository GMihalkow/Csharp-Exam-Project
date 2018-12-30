using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Attributes.CustomValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class AllowedImageExtensions : ValidationAttribute
    {
        private IAccountService accountService;

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

            this.accountService = (IAccountService)validationContext
                   .GetService(typeof(IAccountService));

            var result = this.accountService.IsImageExtensionValid(image.FileName);

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