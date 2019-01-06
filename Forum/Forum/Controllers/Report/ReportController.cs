using Forum.Services.Common;
using Forum.Services.Interfaces.Account;
using Forum.Services.Interfaces.Pagging;
using Forum.Services.Interfaces.Post;
using Forum.Services.Interfaces.Report;
using Forum.Services.Interfaces.Report.Post;
using Forum.Services.Interfaces.Report.Quote;
using Forum.Services.Interfaces.Report.Reply;
using Forum.ViewModels.Report;
using Forum.Web.Attributes.CustomAuthorizeAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Forum.Web.Controllers.Report
{
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IPaggingService paggingService;
        private readonly IPostService postService;
        private readonly IReportService reportService;
        private readonly IPostReportService postReportService;
        private readonly IReplyReportService replyReportService;
        private readonly IQuoteReportService quoteReportService;

        public ReportController(IPaggingService paggingService, IPostService postService, IAccountService accountService, IReportService reportService, IPostReportService postReportService, IReplyReportService replyReportService, IQuoteReportService quoteReportService)
            : base(accountService)
        {
            this.paggingService = paggingService;
            this.postService = postService;
            this.reportService = reportService;
            this.postReportService = postReportService;
            this.replyReportService = replyReportService;
            this.quoteReportService = quoteReportService;
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public IActionResult All()
        {
            return this.View();
        }        
    }
}