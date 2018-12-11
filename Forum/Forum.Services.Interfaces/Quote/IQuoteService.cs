using Forum.Models;
using Forum.ViewModels.Interfaces.Quote;
using System.Collections.Generic;

namespace Forum.Services.Interfaces.Quote
{
    public interface IQuoteService
    {
        int Add(IQuoteInputModel model, ForumUser user);

        IEnumerable<IQuoteViewModel> GetQuotesByPost(string id);
    }
}