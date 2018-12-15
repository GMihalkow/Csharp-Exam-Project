namespace Forum.ViewModels.Interfaces.Report
{
    public interface IReplyReportInputModel
    {
        string ReplyId { get; }

        string Description { get; }

        string PostId { get; }
    }
}