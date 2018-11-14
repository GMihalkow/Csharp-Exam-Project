﻿using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class Reply
    {
        public string Id { get; set; }

        public ForumUser Author { get; set; }

        public string AuthorId { get; set; }

        public string Description { get; set; }

        public ForumUser Reciever { get; set; }

        public string RecieverId { get; set; }

        public Post Post { get; set; }

        public string PostId { get; set; }

        public ICollection<Quote> Quotes { get; set; }

        public DateTime RepliedOn { get; set; }
    }
}