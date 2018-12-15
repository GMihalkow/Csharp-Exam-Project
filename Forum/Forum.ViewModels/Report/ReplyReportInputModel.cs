using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.ViewModels.Interfaces.Report;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Report
{
    public class ReplyReportInputModel : IReplyReportInputModel, IMapTo<ReplyReport>
    {
        public string ReplyId { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public string PostId { get; set; }
    }
}