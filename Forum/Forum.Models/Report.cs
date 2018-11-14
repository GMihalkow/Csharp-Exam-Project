namespace Forum.Models
{
    using System;

    public class Report
    {
        public string Id { get; set; }

        public ForumUser User { get; set; }

        public string UserId { get; set; }

        public Post Post { get; set; }

        public string PostId { get; set; }

        public DateTime OpenedOn { get; set; }
    }
}