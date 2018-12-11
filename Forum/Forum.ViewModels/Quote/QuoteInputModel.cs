using Forum.MapConfiguration.Contracts;
using Forum.ViewModels.Interfaces.Quote;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Quote
{
    public class QuoteInputModel : IQuoteInputModel, IMapTo<Models.Quote>
    {
        public string Id { get; set; }

        public string ReplyId { get; set; }

        public string Quote { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Descrption { get; set; }

        public string RecieverId { get; set; }
    }
}