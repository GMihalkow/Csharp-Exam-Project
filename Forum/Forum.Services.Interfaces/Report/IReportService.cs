using Forum.ViewModels.Interfaces.Report;
using System.Security.Claims;

namespace Forum.Services.Interfaces.Report
{
    public interface IReportService
    {
        IPostReportInputModel AddPostReport(IPostReportInputModel model, string authorId);

        IReplyReportInputModel AddReplyReport(IReplyReportInputModel model, string authorId);

        IQuoteReportInputModel AddQuoteReport(IQuoteReportInputModel model, string authorId);
    }
}