namespace Forum.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class ForumUser : IdentityUser
    {
        public ICollection<Reply> AuthoredReplies { get; set; }

        public ICollection<Reply> RecievedReplies { get; set; }

        public ICollection<Post> Posts { get; set; }
        
        public string Location { get; set; }

        public DateTime LastActiveOn { get; set; }

        public DateTime RegisteredOn { get; set; }
        
        public string ProfilePicutre { get; set; }

        public ICollection<Quote> AuthoredQuotes { get; set; }

        public ICollection<Quote> RecievedQuotes { get; set; }

        public ICollection<Report> Reports { get; set; }
    }
}