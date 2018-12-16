using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.ViewModels.Interfaces.Report;

namespace Forum.ViewModels.Report
{
    public class QuoteReportInputModel : IQuoteReportInputModel, IMapTo<QuoteReport>
    {
        public string Description { get; set; }

        public string QuoteId { get; set; }

        public string PostId { get; set; }
    }
}