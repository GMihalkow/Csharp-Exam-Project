namespace Forum.Models
{
    public class Quote
    {
        public string Id { get; set; }

        public Reply Reply { get; set; }

        public string ReplyId { get; set; }

        public ForumUser Author { get; set; }

        public string AuthorId { get; set; }

        public string Descrption { get; set; }

        public ForumUser Reciever { get; set; }

        public string RecieverId { get; set; }
    }
}