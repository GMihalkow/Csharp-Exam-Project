using AutoMapper;
using Forum.Models;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Report;
using Forum.ViewModels.Interfaces.Report;
using Forum.ViewModels.Report;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Forum.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;

        public ReportService(IMapper mapper, IDbService dbService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
        }

        public IPostReportInputModel AddPostReport(IPostReportInputModel model, string authorId)
        {
            var report = this.mapper.Map<PostReport>(model);
            report.ReportedOn = DateTime.UtcNow;
            report.AuthorId = authorId;

            this.dbService.DbContext.PostReports.Add(report);
            this.dbService.DbContext.SaveChanges();

            return model;
        }

        public IQuoteReportInputModel AddQuoteReport(IQuoteReportInputModel model, string authorId)
        {
            var report = this.mapper.Map<QuoteReport>(model);
            report.ReportedOn = DateTime.UtcNow;
            report.AuthorId = authorId;

            this.dbService.DbContext.QuoteReports.Add(report);
            this.dbService.DbContext.SaveChanges();

            return model;
        }

        public IReplyReportInputModel AddReplyReport(IReplyReportInputModel model, string authorId)
        {
            var report = this.mapper.Map<ReplyReport>(model);
            report.ReportedOn = DateTime.UtcNow;
            report.AuthorId = authorId;

            this.dbService.DbContext.ReplyReports.Add(report);
            this.dbService.DbContext.SaveChanges();

            return model;
        }

        public int DismissPostReport(string id)
        {
            var report =
                this.dbService
                .DbContext
                .PostReports
                .Where(pr => pr.Id == id)
                .FirstOrDefault();

            if (report == null)
            {
                return 0;
            }

            this.dbService.DbContext.PostReports.Remove(report);
            var result = this.dbService.DbContext.SaveChanges();

            return result;
        }

        public int DismissQuoteReport(string id)
        {
            var report =
                 this.dbService
                 .DbContext
                 .QuoteReports
                 .Where(pr => pr.Id == id)
                 .FirstOrDefault();

            if (report == null)
            {
                return 0;
            }

            this.dbService.DbContext.QuoteReports.Remove(report);
            var result = this.dbService.DbContext.SaveChanges();

            return result;
        }

        public int DismissReplyReport(string id)
        {
            var report =
                   this.dbService
                   .DbContext
                   .ReplyReports
                   .Where(pr => pr.Id == id)
                   .FirstOrDefault();

            if (report == null)
            {
                return 0;
            }

            this.dbService.DbContext.ReplyReports.Remove(report);
            var result = this.dbService.DbContext.SaveChanges();

            return result;
        }

        public IEnumerable<IPostReportViewModel> GetPostReports()
        {
            var reports =
                this.dbService
                .DbContext
                .PostReports
                .Include(pr => pr.Author)
                .Include(pr => pr.Post)
                .ThenInclude(pr => pr.Author)
                .OrderBy(pr => pr.ReportedOn)
                .Select(pr => this.mapper.Map<PostReportViewModel>(pr))
                .ToList();

            return reports;
        }

        public IEnumerable<IQuoteReportViewModel> GetQuoteReports()
        {
            var reports =
                   this.dbService
                   .DbContext
                   .QuoteReports
                   .Include(qr => qr.Author)
                   .Include(qr => qr.Quote)
                   .ThenInclude(qr => qr.Author)
                   .Include(qr => qr.Quote)
                   .ThenInclude(qr => qr.Reply)
                   .OrderBy(qr => qr.ReportedOn)
                   .Select(qr => this.mapper.Map<QuoteReportViewModel>(qr))
                   .ToList();

            return reports;
        }

        public IEnumerable<IReplyReportViewModel> GetReplyReports()
        {
            var reports =
                   this.dbService
                   .DbContext
                   .ReplyReports
                   .Include(rr => rr.Author)
                   .Include(rr => rr.Reply)
                   .ThenInclude(rr => rr.Author)
                   .OrderBy(rr => rr.ReportedOn)
                   .Select(rr => this.mapper.Map<ReplyReportViewModel>(rr))
                   .ToList();

            return reports;
        }

        public int DeleteUserReports(string username)
        {
            var postReports =
                this.dbService
                .DbContext
                .PostReports
                .Include(pr => pr.Author)
                .Where(pr => pr.Author.UserName == username)
                .ToList();

            this.dbService.DbContext.PostReports.RemoveRange(postReports);

            var replyReports =
                this.dbService
                .DbContext
                .ReplyReports
                .Include(pr => pr.Author)
                .Where(pr => pr.Author.UserName == username)
                .ToList();

            this.dbService.DbContext.ReplyReports.RemoveRange(replyReports);

            var quoteReports =
                this.dbService
                .DbContext
                .QuoteReports
                .Include(pr => pr.Author)
                .Where(pr => pr.Author.UserName == username)
                .ToList();

            this.dbService.DbContext.QuoteReports.RemoveRange(quoteReports);

            return this.dbService.DbContext.SaveChanges();
        }
    }
}