namespace Forum.Web.ViewModels.Home
{
    public class IndexInfoViewModel
    {
        public Forum.Models.Category[] Categories { get; set; }

        public int TotalUsersCount { get; set; }

        public int TotalPostsCount { get; set; }

        public string NewestUser { get; set; }
    }
}