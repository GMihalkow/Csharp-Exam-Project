namespace Forum.Models
{
    using System;

    public class Report
    {
        public string Id { get; set; }

        public ForumUser Author { get; set; }

        public string AuthorId { get; set; }

        public string Description { get; set; }

        public ForumUser Reciever { get; set; }

        public string RecieverId { get; set; }

        public Post Post { get; set; }

        public string PostId { get; set; }

        public DateTime OpenedOn { get; set; }
    }
}