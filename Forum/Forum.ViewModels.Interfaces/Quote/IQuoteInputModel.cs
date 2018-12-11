namespace Forum.ViewModels.Interfaces.Quote
{
    public interface IQuoteInputModel
    {
        string Id { get; }

        string ReplyId { get; }

        string Quote { get; }

        string Descrption { get; }

        string RecieverId { get; }
    }
}