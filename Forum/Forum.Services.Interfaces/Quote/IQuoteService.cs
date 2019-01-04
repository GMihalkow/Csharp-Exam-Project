using Forum.Models;
using Forum.ViewModels.Interfaces.Quote;
using System.Collections.Generic;

namespace Forum.Services.Interfaces.Quote
{
    public interface IQuoteService
    {
        int Add(IQuoteInputModel model, ForumUser user, string recieverName);

        IEnumerable<IQuoteViewModel> GetQuotesByPost(string id);

        Models.Quote GetQuote(string id);

        int DeleteUserQuotes(ForumUser user);
    }
}