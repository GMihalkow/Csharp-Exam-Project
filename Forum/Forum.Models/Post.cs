using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class Post
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Reply> Replies { get; set; }

        public string Description { get; set; }

        public DateTime StartedOn { get; set; }

        public ForumUser Author { get; set; }

        public string AuthorId { get; set; }
        
        public SubForum Forum { get; set; }

        public string ForumId { get; set; }

        public ICollection<Report> Reports { get; set; }
    }
}