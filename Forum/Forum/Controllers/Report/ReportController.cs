using Forum.Services.Interfaces.Report;
using Forum.ViewModels.Report;
using Forum.Web.Attributes.CustomAuthorizeAttributes;
using Forum.Web.Common;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            if (this.ModelState.IsValid)
            {
                string authorId = this.accountService.GetUser(this.User).Id;

                this.reportService.AddPostReport(model, authorId);
                return this.Redirect($"/Post/Details?id={model.PostId}");
            }
            else
            {
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
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
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
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
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
            }
        }
        
        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public IActionResult All()
        {
            return this.View();
        }
        
        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult GetPostReports(int start)
        {
            var reports = this.reportService.GetPostReports(start);

            this.ViewData["PostReportsCount"] = this.reportService.GetPostReportsCount();

            this.ViewData["PagesCount"] = this.reportService.GetPagesCount(this.reportService.GetPostReportsCount());

            return this.PartialView("~/Views/Report/Post/_PostReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult GetReplyReports(int start)
        {
            var reports = this.reportService.GetReplyReports(start);

            this.ViewData["ReplyReportsCount"] = this.reportService.GetReplyReportsCount();

            this.ViewData["PagesCount"] = this.reportService.GetPagesCount(this.reportService.GetReplyReportsCount());

            return this.PartialView("~/Views/Report/Reply/_ReplyReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult GetQuoteReports(int start)
        {
            var reports = this.reportService.GetQuoteReports(start);

            this.ViewData["QuoteReportsCount"] = this.reportService.GetQuoteReportsCount();

            this.ViewData["PagesCount"] = this.reportService.GetPagesCount(this.reportService.GetQuoteReportsCount());

            return this.PartialView("~/Views/Report/Quote/_QuoteReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissPostReport(string id)
        {
            this.reportService.DismissPostReport(id);

            var reports = this.reportService.GetPostReports(0);

            this.ViewData["PostReportsCount"] = this.reportService.GetPostReportsCount();

            this.ViewData["PagesCount"] = this.reportService.GetPagesCount(this.reportService.GetPostReportsCount());

            return this.PartialView("~/Views/Report/Post/_PostReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissReplyReport(string id)
        {
            this.reportService.DismissReplyReport(id);

            var reports = this.reportService.GetReplyReports(0);

            this.ViewData["ReplyReportsCount"] = this.reportService.GetReplyReportsCount();

            this.ViewData["PagesCount"] = this.reportService.GetPagesCount(this.reportService.GetReplyReportsCount());

            return this.PartialView("~/Views/Report/Reply/_ReplyReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissQuoteReport(string id)
        {
            this.reportService.DismissQuoteReport(id);

            var reports = this.reportService.GetQuoteReports(0);

            this.ViewData["QuoteReportsCount"] = this.reportService.GetQuoteReportsCount();

            this.ViewData["PagesCount"] = this.reportService.GetPagesCount(this.reportService.GetQuoteReportsCount());
            
            return this.PartialView("~/Views/Report/Quote/_QuoteReportsPartial.cshtml", reports);
        }
    }
}