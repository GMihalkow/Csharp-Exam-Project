using AutoMapper;
using Forum.Models;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Report.Quote;
using Forum.ViewModels.Interfaces.Report;
using Forum.ViewModels.Report;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Forum.Services.Report.Quote
{
    public class QuoteReportService : IQuoteReportService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;

        public QuoteReportService(IMapper mapper, IDbService dbService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
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

        public int DismissQuoteReport(string id)
        {
            var report = this.dbService.DbContext.QuoteReports.Where(pr => pr.Id == id).FirstOrDefault();

            if (report == null)
            {
                return 0;
            }

            this.dbService.DbContext.QuoteReports.Remove(report);
            var result = this.dbService.DbContext.SaveChanges();

            return result;
        }

        public IEnumerable<IQuoteReportViewModel> GetQuoteReports(int start)
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
                   .Skip(start)
                   .Take(5)
                   .Select(qr => this.mapper.Map<QuoteReportViewModel>(qr))
                   .ToList();

            return reports;
        }

        public int GetQuoteReportsCount()
        {
            var count = this.dbService.DbContext.QuoteReports.Count();

            return count;
        }
    }
}