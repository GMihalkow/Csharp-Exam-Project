using Forum.Data.EntityConfiguration;
using Forum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data
{
    public class ForumDbContext : IdentityDbContext<ForumUser>
    {
        public ForumDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubForum> Forums { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=ForumDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration<Report>(new ReportConfiguration());
            builder.ApplyConfiguration<Quote>(new QuoteConfiguration());
            builder.ApplyConfiguration<Post>(new PostConfiguration());
            builder.ApplyConfiguration<Reply>(new ReplyConfiguration());
            builder.ApplyConfiguration<Category>(new CategoryConfiguration());
        }
    }
}