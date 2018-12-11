using Forum.Models;

namespace Forum.ViewModels.Interfaces.Quote
{
    public interface IQuoteViewModel
    {
        ForumUser Author { get; }

        Models.Reply Reply { get; }

        string Descrption { get; }
    }
}