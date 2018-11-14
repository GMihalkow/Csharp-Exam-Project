using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class SubForum
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }

        public string Description { get; set; }

        public Categorie Categorie { get; set; }

        public string CategorieId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
