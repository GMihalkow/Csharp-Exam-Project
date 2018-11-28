namespace Forum.Services.Db
{
    using global::Forum.Data;

    public class DbService 
    {
        public DbService(ForumDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ForumDbContext DbContext { get; }
    }
}