namespace Forum.Web.Attributes.CustomValidationAttributes
{
    using Forum.Services.Db;
    using Forum.Services.Interfaces.Category;
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class CategoriesExistAttribute : ValidationAttribute
    {
        private DbService dbService;
        private ICategoryService categoryService;

        public CategoriesExistAttribute()
        {
        }

        public CategoriesExistAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            this.dbService = (DbService)validationContext
                   .GetService(typeof(DbService));

            this.categoryService = (ICategoryService)validationContext
                   .GetService(typeof(ICategoryService));

            if(this.categoryService.IsCategoryValid(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid category.");
            }
        }
    }
}