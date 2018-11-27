using Forum.Models;

namespace Forum.Web.ViewModels.Post
{
    //TODO: Add validation
    public class PostInputModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ForumId { get; set; }

        public string ForumName { get; set; }
    }
}