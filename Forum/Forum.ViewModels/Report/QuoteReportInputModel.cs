using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Quote;
using Forum.ViewModels.Interfaces.Report;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Report
{
    public class QuoteReportInputModel : IQuoteReportInputModel, IMapTo<QuoteReport>, IValidatableObject
    {
        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public string QuoteId { get; set; }

        public string PostId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} must be between {1} and {2} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var postService = (IPostService)validationContext
                   .GetService(typeof(IPostService));

            var quoteService = (IQuoteService)validationContext
                .GetService(typeof(IQuoteService));

            var model = validationContext.ObjectInstance as QuoteReportInputModel;

            var quote = quoteService.GetQuote(model.QuoteId);
            if(quote == null)
            {
                yield return new ValidationResult("Error. Invalid quote id.");
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