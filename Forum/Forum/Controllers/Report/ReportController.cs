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

        [HttpPost]
        public IActionResult ReportPost(PostReportInputModel model)
        {
            var post = this.postService.GetPost(model.PostId, 0, this.ModelState);

            if (this.ModelState.IsValid)
            {
                string authorId = this.accountService.GetUser(this.User).Id;

                this.postReportService.AddPostReport(model, authorId);
                return this.Redirect($"/Post/Details?id={post.Id}");
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
            var post = this.postService.GetPost(model.PostId, 0, this.ModelState);

            if (ModelState.IsValid)
            {
                string authorId = this.accountService.GetUser(this.User).Id;

                this.replyReportService.AddReplyReport(model, authorId);
                return this.Redirect($"/Post/Details?id={post.Id}");
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
            var post = this.postService.GetPost(model.PostId, 0, this.ModelState);

            if (ModelState.IsValid)
            {
                string authorId = this.accountService.GetUser(this.User).Id;

                this.quoteReportService.AddQuoteReport(model, authorId);
                return this.Redirect($"/Post/Details?id={post.Id}");
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
            var reports = this.postReportService.GetPostReports(start);

            this.ViewData["PostReportsCount"] = this.postReportService.GetPostReportsCount();

            this.ViewData["PagesCount"] = this.paggingService.GetPagesCount(this.postReportService.GetPostReportsCount());

            return this.PartialView("~/Views/Report/Post/_PostReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult GetReplyReports(int start)
        {
            var reports = this.replyReportService.GetReplyReports(start);

            this.ViewData["ReplyReportsCount"] = this.replyReportService.GetReplyReportsCount();

            this.ViewData["PagesCount"] = this.paggingService.GetPagesCount(this.replyReportService.GetReplyReportsCount());

            return this.PartialView("~/Views/Report/Reply/_ReplyReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult GetQuoteReports(int start)
        {
            var reports = this.quoteReportService.GetQuoteReports(start);

            this.ViewData["QuoteReportsCount"] = this.quoteReportService.GetQuoteReportsCount();

            this.ViewData["PagesCount"] = this.paggingService.GetPagesCount(this.quoteReportService.GetQuoteReportsCount());

            return this.PartialView("~/Views/Report/Quote/_QuoteReportsPartial.cshtml", reports);
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissPostReport(string id)
        {
            this.postReportService.DismissPostReport(id, this.ModelState);
            if (this.ModelState.IsValid)
            {
                var reports = this.postReportService.GetPostReports(0);

                this.ViewData["PostReportsCount"] = this.postReportService.GetPostReportsCount();

                this.ViewData["PagesCount"] = this.paggingService.GetPagesCount(this.postReportService.GetPostReportsCount());

                return this.PartialView("~/Views/Report/Post/_PostReportsPartial.cshtml", reports);
            }
            else
            {
                var result = this.PartialView("_ErrorPartial", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.NotFound;

                return result;
            }
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissReplyReport(string id)
        {
            this.replyReportService.DismissReplyReport(id,this.ModelState);
            if (this.ModelState.IsValid)
            {
                var reports = this.replyReportService.GetReplyReports(0);

                this.ViewData["ReplyReportsCount"] = this.replyReportService.GetReplyReportsCount();

                this.ViewData["PagesCount"] = this.paggingService.GetPagesCount(this.replyReportService.GetReplyReportsCount());

                return this.PartialView("~/Views/Report/Reply/_ReplyReportsPartial.cshtml", reports);
            }
            else
            {
                var result = this.PartialView("_ErrorPartial", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.NotFound;

                return result;
            }
        }

        [AuthorizeRoles(Role.Administrator, Role.Owner)]
        public PartialViewResult DismissQuoteReport(string id)
        {
            this.quoteReportService.DismissQuoteReport(id, this.ModelState);

            if (this.ModelState.IsValid)
            {
                var reports = this.quoteReportService.GetQuoteReports(0);

                this.ViewData["QuoteReportsCount"] = this.quoteReportService.GetQuoteReportsCount();

                this.ViewData["PagesCount"] = this.paggingService.GetPagesCount(this.quoteReportService.GetQuoteReportsCount());

                return this.PartialView("~/Views/Report/Quote/_QuoteReportsPartial.cshtml", reports);
            }
            else
            {
                var result = this.PartialView("_ErrorPartial", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.NotFound;

                return result;
            }
        }
    }
}