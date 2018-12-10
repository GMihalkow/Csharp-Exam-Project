using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class Category
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ForumUser User { get; set; }
        
        public string UserId { get; set; }

        public ICollection<SubForum> Forums { get; set; }

        public CategoryType Type { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ForumsCount => this.Forums.Count;
    }
}