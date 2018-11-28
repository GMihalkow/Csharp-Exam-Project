namespace Forum.Web.ViewModels.Forum
{
    using System.ComponentModel.DataAnnotations;

    public class ForumInputModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z_\-0-9]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-'")]
        [StringLength(50, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_\/\-0-9!.?()&]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-', '(', ')', '&', '.', '/', '?', '!'")]
        [StringLength(50, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Description { get; set; }
        
        //TODO: should hive Id maybe
        public string Category { get; set; }
    }
}