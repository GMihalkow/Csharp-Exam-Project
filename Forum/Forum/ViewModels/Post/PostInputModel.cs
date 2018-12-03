namespace Forum.Web.ViewModels.Post
{
    using global::Forum.MapConfiguration.Contracts;
    using System.ComponentModel.DataAnnotations;
    
    public class PostInputModel : IMapTo<global::Forum.Models.Post>
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z_\-0-9]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-'")]
        [StringLength(50, ErrorMessage ="{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Description { get; set; }

        public string ForumId { get; set; }

        public string ForumName { get; set; }
    }
}