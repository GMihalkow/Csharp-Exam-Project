using Forum.MapConfiguration.Contracts;
using Forum.ViewModels.Interfaces.Post;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Post
{    
    public class PostInputModel : IPostInputModel, IMapTo<Models.Post>
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z_\-0-9]*$", ErrorMessage = "{0} is allowed to contain only lowercase/uppercase characters, digits and '_', '-'")]
        [StringLength(50, ErrorMessage ="{0} length must be between {1} and {2} characters.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public string ForumId { get; set; }

        public string ForumName { get; set; }
    }
}