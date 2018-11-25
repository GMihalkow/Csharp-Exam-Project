namespace Forum.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class ForumUser : IdentityUser
    {
        public string Location { get; set; }

        public string Gender { get; set; }

        public DateTime LastActiveOn { get; set; }

        public DateTime RegisteredOn { get; set; }

        public string ProfilePicutre { get; set; }

        public ICollection<Reply> Replies { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Quote> Quotes { get; set; }

        public ICollection<Report> Reports { get; set; }
    }
}