namespace Forum.Data.EntityConfiguration
{
    using Forum.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            builder
                .HasKey(q => q.Id);

            builder
                .HasOne(q => q.Reply)
                .WithMany(r => r.Quotes)
                .HasForeignKey(q => q.ReplyId);

            builder
                .HasOne(q => q.Reciever)
                .WithMany(u => u.RecievedQuotes)
                .HasForeignKey(q => q.RecieverId);

            builder
                .HasOne(q => q.Author)
                .WithMany(u => u.AuthoredQuotes)
                .HasForeignKey(q => q.AuthorId);
        }
    }
}