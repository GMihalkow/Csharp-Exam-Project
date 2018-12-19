using Forum.ViewModels.Interfaces.Report;
using System.Collections.Generic;

namespace Forum.Services.Interfaces.Report
{
    public interface IReportService
    {
        IPostReportInputModel AddPostReport(IPostReportInputModel model, string authorId);

        IReplyReportInputModel AddReplyReport(IReplyReportInputModel model, string authorId);

        IQuoteReportInputModel AddQuoteReport(IQuoteReportInputModel model, string authorId);

        IEnumerable<IPostReportViewModel> GetPostReports();

        IEnumerable<IReplyReportViewModel> GetReplyReports();

        IEnumerable<IQuoteReportViewModel> GetQuoteReports();

        int DismissPostReport(string id);

        int DismissReplyReport(string id);

        int DismissQuoteReport(string id);
    }
}