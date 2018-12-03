namespace Forum.ViewModels.Home
{
    using global::Forum.Models;

    public class IndexInfoViewModel
    {
        public Category[] Categories { get; set; }

        public int TotalUsersCount { get; set; }

        public int TotalPostsCount { get; set; }

        public string NewestUser { get; set; }
    }
}