namespace Forum.ViewModels.Interfaces.Quote
{
    public interface IQuoteInputModel
    {
        string Id { get; }

        string ReplyId { get; }

        string Description { get; }

        string Quote { get; }

        string RecieverId { get; }

        string RecieverName { get; }
    }
}