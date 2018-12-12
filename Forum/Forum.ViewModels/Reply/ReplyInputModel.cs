using Forum.Models;
using Forum.Services.Interfaces.Post;
using Forum.ViewModels.Interfaces.Reply;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Reply
{
    public class ReplyInputModel : IReplyInputModel, IValidatableObject
    {
        private IPostService postService;

        public string Id { get; set; }

        public ForumUser Author { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        [Required]
        public string PostId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            this.postService = (IPostService)validationContext
                   .GetService(typeof(IPostService));

            var model = (ReplyInputModel)validationContext.ObjectInstance;

            if (this.postService.DoesPostExist(model.PostId))
            {
                yield return ValidationResult.Success;
            }
            else
            {
                yield return new ValidationResult("Invalid post id.");
            }
        }
    }
}