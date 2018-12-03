namespace Forum.Web.ViewModels.Forum
{
    using System.Collections.Generic;
    using global::Forum.MapConfiguration.Contracts;
    using global::Forum.Models;
    
    public class ForumPostsInputModel : IMapFrom<SubForum>
    {
        public SubForum Forum { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}