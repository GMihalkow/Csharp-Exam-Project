using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class Categorie
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<SubForum> Forums { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}