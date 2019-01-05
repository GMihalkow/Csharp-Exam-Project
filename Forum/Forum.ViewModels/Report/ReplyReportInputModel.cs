using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Reply;
using Forum.ViewModels.Common;
using Forum.ViewModels.Interfaces.Report;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Report
{
    public class ReplyReportInputModel : IReplyReportInputModel, IMapTo<ReplyReport>, IValidatableObject
    {
        public string ReplyId { get; set; }

        [Required(ErrorMessage = ErrorConstants.RequiredError)]
        [MinLength(ErrorConstants.MinimumDescriptionLength, ErrorMessage = ErrorConstants.MinimumLengthError)]
        public string Description { get; set; }
        
        public string PostId { get; set; }

        [Required(ErrorMessage = ErrorConstants.RequiredError)]
        [StringLength(ErrorConstants.MaximumNamesLength, ErrorMessage = ErrorConstants.StringLengthErrorMessage, MinimumLength = ErrorConstants.MinimumNamesLength)]
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
                yield return new ValidationResult(ErrorConstants.InvalidReplyIdError);
            }

            if (postService.DoesPostExist(model.PostId))
            {
                yield return ValidationResult.Success;
            }
            else
            {
                yield return new ValidationResult(ErrorConstants.InvalidPostIdError);
            }
        }
    }
}