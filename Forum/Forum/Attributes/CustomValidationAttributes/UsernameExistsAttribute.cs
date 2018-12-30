namespace Forum.Web.Attributes.CustomValidationAttributes
{
    using Forum.Services.Db;
    using Forum.Web.Services.Account.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class UsernameExistsAttribute : ValidationAttribute
    {
        private DbService dbService;

        private IAccountService accountService;

        public UsernameExistsAttribute()
        {

        }
        
        public UsernameExistsAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            this.dbService = (DbService)validationContext
                .GetService(typeof(DbService));

            this.accountService = (IAccountService)validationContext
                .GetService(typeof(IAccountService));
            
            if (!this.accountService.UsernameExists(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Username already exists.");
            }
        }
    }
}