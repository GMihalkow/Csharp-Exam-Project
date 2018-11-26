namespace Forum.Web.Services
{
    using Forum.Data;

    public class DbService 
    {
        public DbService(ForumDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ForumDbContext DbContext { get; }
    }
}