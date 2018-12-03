namespace Forum.Services.Db
{
    using global::Forum.Data;
    using global::Forum.Services.Interfaces.Db;

    public class DbService : IDbService
    {
        public DbService(ForumDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ForumDbContext DbContext { get; }
    }
}