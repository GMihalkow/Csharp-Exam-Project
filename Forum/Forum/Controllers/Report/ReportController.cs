﻿using Forum.Services.Interfaces.Report;
using Forum.ViewModels.Report;
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

        public IActionResult All()
        {
            return this.View();
        }

        public PartialViewResult GetAllReports()
        {
            return PartialView("_PostReportsPartial");
        }
    }
}