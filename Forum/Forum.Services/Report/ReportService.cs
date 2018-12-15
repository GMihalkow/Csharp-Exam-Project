using AutoMapper;
using Forum.Models;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Report;
using Forum.ViewModels.Interfaces.Report;
using System;
using System.Security.Claims;

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

        public IReplyReportInputModel AddReplyReport(IReplyReportInputModel model, string authorId)
        {
            var report = this.mapper.Map<ReplyReport>(model);
            report.ReportedOn = DateTime.UtcNow;
            report.AuthorId = authorId;
            
            this.dbService.DbContext.ReplyReports.Add(report);
            this.dbService.DbContext.SaveChanges();

            return model;
        }
    }
}