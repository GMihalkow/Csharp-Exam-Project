using Forum.Models;
using Forum.ViewModels.Interfaces.Quote;

namespace Forum.Services.Interfaces.Quote
{
    public interface IQuoteService
    {
        int Add(IQuoteInputModel model, ForumUser user);
    }
}