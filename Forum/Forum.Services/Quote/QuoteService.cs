using AutoMapper;
using Forum.Models;
using Forum.Services.Interfaces.Db;
using Forum.ViewModels.Interfaces.Quote;

namespace Forum.Services.Quote
{
    public class QuoteService : Interfaces.Quote.IQuoteService
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
            
            this.dbService.DbContext.Quotes.Add(quote);
            return this.dbService.DbContext.SaveChanges();
        }
    }
}