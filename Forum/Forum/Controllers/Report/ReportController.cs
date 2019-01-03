using Forum.Services.Interfaces.Report;
using Forum.ViewModels.Report;
using Forum.Web.Attributes.CustomAuthorizeAttributes;
using Forum.Web.Common;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers.Report
{
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IReportService reportService;

        public ReportController(IAccountService accountService, IReportService reportService)
            : base(accountService)
        {
            this.reportService = reportService;
        }

        [HttpPost]
        public IActionResult ReportPost(PostReportInputModel model)
        {
            if (ModelState.IsValid)
            {
                string authorId = this.accountService.GetUser(this.User).Id;

                this.reportService.AddPostReport(model, authorId);
                return this.Redirect($"/Post/Details?id={model.PostId}");
            }
            else
            {
                //TODO: Show errors correctly
                return this.Redirect("/");
            }
        }

        [HttpPost]
        public IActionResult ReportReply(ReplyReportInputModel model)
        {
            if (ModelState.IsValid)
            {
                string authorId = this.accountService.GetUser(this.User).Id;

                this.reportService.AddReplyReport(model, authorId);
                return this.Redirect($"/Post/Details?id={model.PostId}");
            }
            else
            {
                //TODO: Show errors correctly
                return this.Redirect("/");
            }
        }

        [HttpPost]
        public IActionResult ReportQuote(QuoteReportInputModel model)
        {
            if (ModelState.IsValid)
            {
                string authorId = this.accountService.GetUser(this.User).Id;

                this.reportService.AddQuoteReport(model, authorId);
                return this.Redirect($"/Post/Details?id={model.PostId}");
            }
            else
            {
                //TODO: Show errors correctly
                return this.Redirect("/");
            }
        }
        
        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public IActionResult All()
        {
            return this.View();
        }
        
        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult GetPostReports()
        {
            var reports = this.reportService.GetPostReports();

            return this.PartialView("~/Views/Report/Post/_PostReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult GetReplyReports()
        {
            var reports = this.reportService.GetReplyReports();

            return this.PartialView("~/Views/Report/Reply/_ReplyReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult GetQuoteReports()
        {
            var reports = this.reportService.GetQuoteReports();

            return this.PartialView("~/Views/Report/Quote/_QuoteReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissPostReport(string id)
        {
            this.reportService.DismissPostReport(id);

            var reports = this.reportService.GetPostReports();

            return this.PartialView("~/Views/Report/Post/_PostReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissReplyReport(string id)
        {
            this.reportService.DismissReplyReport(id);

            var reports = this.reportService.GetReplyReports();

            return this.PartialView("~/Views/Report/Reply/_ReplyReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissQuoteReport(string id)
        {
            this.reportService.DismissQuoteReport(id);

            var reports = this.reportService.GetQuoteReports();

            return this.PartialView("~/Views/Report/Quote/_QuoteReportsPartial.cshtml", reports);
        }
    }
}