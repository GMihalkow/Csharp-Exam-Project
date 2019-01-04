using Forum.ViewModels.Interfaces.Report;
using System.Collections.Generic;

namespace Forum.Services.Interfaces.Report.Post
{
    public interface IPostReportService
    {
        IPostReportInputModel AddPostReport(IPostReportInputModel model, string authorId);

        IEnumerable<IPostReportViewModel> GetPostReports(int start);

        int DismissPostReport(string id);

        int GetPostReportsCount();
    }
}