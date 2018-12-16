namespace Forum.ViewModels.Interfaces.Report
{
    public interface IQuoteReportInputModel
    {
        string Description { get; }

        string QuoteId { get; }

        string PostId { get; }
    }
}