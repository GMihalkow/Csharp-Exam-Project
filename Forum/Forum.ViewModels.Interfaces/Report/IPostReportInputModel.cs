namespace Forum.ViewModels.Interfaces.Report
{
    public interface IPostReportInputModel
    {
        string Description { get; }

        string PostId { set; }
    }
}