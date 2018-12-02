namespace Forum.Attributes
{
    using Forum.Services.Db;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Property)]
    public class UsernameExistsAttribute : ValidationAttribute
    {
        private DbService dbService;

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

            //TODO: All bussiness logic must be in services. Extract it there.
            //TODO: Change "Admin" to "Administrator"   
            //TODO: Create validationa attributes for select tags
            if (!(this.dbService.DbContext.Users.Any(u => u.UserName == value.ToString())))
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