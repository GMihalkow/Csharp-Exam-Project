using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.ViewModels.Interfaces.Quote;

namespace Forum.ViewModels.Quote
{
    public class QuoteViewModel : IQuoteViewModel, IMapFrom<Models.Quote>
    {
        public ForumUser Author { get; set; }

        public Models.Reply Reply { get; set; }

        public string Descrption { get; set; }
    }
}