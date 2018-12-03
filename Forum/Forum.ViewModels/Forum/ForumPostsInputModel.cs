namespace Forum.ViewModels.Forum
{
    using System.Collections.Generic;
    using global::Forum.MapConfiguration.Contracts;
    using global::Forum.Models;
    using global::Forum.ViewModels.Interfaces;

    public class ForumPostsInputModel : IForumPostsInputModel, IMapFrom<SubForum>
    {
        public SubForum Forum { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}