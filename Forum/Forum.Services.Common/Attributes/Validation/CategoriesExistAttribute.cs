namespace Forum.Services.Common.Attributes.Validation
{
    using Forum.Services.Interfaces.Category;
    using Forum.Services.Interfaces.Db;
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class CategoriesExistAttribute : ValidationAttribute
    {
        private IDbService dbService;
        private ICategoryService categoryService;

        public CategoriesExistAttribute()
        {
        }

        public CategoriesExistAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbService = (IDbService)validationContext
                   .GetService(typeof(IDbService));

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