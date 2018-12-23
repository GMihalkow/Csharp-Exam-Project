using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.ViewModels.Interfaces.Quote;

namespace Forum.ViewModels.Quote
{
    public class QuoteInputModel : IQuoteInputModel, IMapTo<Models.Quote>
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string Quote { get; set; }

        public string RecieverId { get; set; }

        public string QuoteRecieverId { get; set; }

        public string ReplyId { get; set; }
    }
}