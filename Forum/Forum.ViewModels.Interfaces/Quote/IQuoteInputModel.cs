using Forum.MapConfiguration.Contracts;

namespace Forum.ViewModels.Interfaces.Quote
{
    public interface IQuoteInputModel
    {
        string Id { get; }

        string ReplyId { get; }

        string Quote { get; }

        string Description { get; }

        string RecieverId { get; }
    }
}