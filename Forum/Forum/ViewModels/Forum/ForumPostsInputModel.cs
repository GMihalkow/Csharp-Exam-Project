namespace Forum.Web.ViewModels.Forum
{
    using System.Collections.Generic;
    using global::Forum.Models;

    //TODO: Add validation
    public class ForumPostsInputModel
    {
        public SubForum Forum { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}