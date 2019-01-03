using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Post;
using Forum.ViewModels.Interfaces.Quote;
using Forum.ViewModels.Quote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Forum.Services.Quote
{
    [Authorize]
    public class QuoteService : Interfaces.Quote.IQuoteService, IMapTo<Models.Quote>
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;

        public QuoteService(IMapper mapper, IDbService dbService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
        }

        public int Add(IQuoteInputModel model, ForumUser user, string recieverName)
        {
            var quote = this.mapper.Map<Models.Quote>(model);
            quote.Id = Guid.NewGuid().ToString();
            quote.Author = user;
            quote.AuthorId = user.Id;
            quote.QuotedOn = DateTime.UtcNow;
            
            this.dbService.DbContext.Quotes.Add(quote);
            return this.dbService.DbContext.SaveChanges();
        }

        public Models.Quote GetQuote(string id)
        {
            var quote =
                this.dbService
                .DbContext
                .Quotes
                .Include(q => q.Author)
                .Include(q => q.Reply)
                .ThenInclude(q => q.Author)
                .Include(q => q.Reply.Post)
                .FirstOrDefault(q => q.Id == id);

            return quote;
        }

        public IEnumerable<IQuoteViewModel> GetQuotesByPost(string id)
        {
            var quotes =
                this.dbService
                .DbContext
                .Quotes
                .Include(q => q.Author)
                .ThenInclude(q => q.Posts)
                .Include(q => q.Reply)
                .Where(q => q.Reply.PostId == id)
                .Select(q => mapper.Map<QuoteViewModel>(q));

            return quotes;
        }
    }
}