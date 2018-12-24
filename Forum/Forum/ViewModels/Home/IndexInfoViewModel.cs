namespace Forum.Web.ViewModels.Home
{
    using Forum.ViewModels.Interfaces.Post;
    using Forum.ViewModels.Post;
    using global::Forum.Models;
    using System.Collections.Generic;

    public class IndexInfoViewModel
    {
        public Category[] Categories { get; set; }

        public int TotalUsersCount { get; set; }

        public int TotalPostsCount { get; set; }

        public string NewestUser { get; set; }

        public IEnumerable<ILatestPostViewModel> LatestPosts { get; set; }

        public IEnumerable<IPopularPostViewModel> PopularPosts { get; set; }
    }
}