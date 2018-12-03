namespace Forum.Attributes
{
    using Forum.Services.Account;
    using Forum.Services.Account.Contracts;
    using Forum.Services.Db;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

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
            
            //TODO: Change "Admin" to "Administrator"
            
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