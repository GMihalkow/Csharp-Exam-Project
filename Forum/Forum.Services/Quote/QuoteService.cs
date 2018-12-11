using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Forum.MapConfiguration.Contracts;
using Forum.Models;
using Forum.Services.Interfaces.Db;
using Forum.ViewModels.Interfaces.Quote;
using Forum.ViewModels.Quote;
using Microsoft.EntityFrameworkCore;

namespace Forum.Services.Quote
{
    public class QuoteService : Interfaces.Quote.IQuoteService, IMapTo<Models.Quote>
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;

        public QuoteService(IMapper mapper, IDbService dbService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
        }

        public int Add(IQuoteInputModel model, ForumUser user)
        {
            var quote = this.mapper.Map<Models.Quote>(model);
            quote.Author = user;
            quote.AuthorId = user.Id;

            this.dbService.DbContext.Quotes.Add(quote);
            return this.dbService.DbContext.SaveChanges();
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