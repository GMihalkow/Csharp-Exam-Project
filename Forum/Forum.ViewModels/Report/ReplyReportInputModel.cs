using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Interfaces.Report;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Report
{
    public class ReplyReportInputModel : IReplyReportInputModel, IMapTo<ReplyReport>, IValidatableObject
    {
        public string ReplyId { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }
        
        public string PostId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var postService = (IPostService)validationContext
                   .GetService(typeof(IPostService));

            var replyService = (IReplyService)validationContext
                .GetService(typeof(IReplyService));
            
            var model = validationContext.ObjectInstance as ReplyReportInputModel;

            var reply = replyService.GetReply(model.ReplyId);
            if (reply == null)
            {
                yield return new ValidationResult("Error. Invalid reply id.");
            }

            if (postService.DoesPostExist(model.PostId))
            {
                yield return ValidationResult.Success;
            }
            else
            {
                yield return new ValidationResult("Error. Invalid post id.");
            }
        }
    }
}