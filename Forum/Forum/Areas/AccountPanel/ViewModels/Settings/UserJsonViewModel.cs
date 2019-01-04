using Forum.MapConfiguration.Contracts;
using Forum.Models;
using System;

namespace Forum.Web.Areas.Settings.ViewModels.Settings
{
    public class UserJsonViewModel : IMapFrom<ForumUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public DateTime RegisteredOn { get; set; }

        public string Gender { get; set; }

        public string Location { get; set; }
    }
}