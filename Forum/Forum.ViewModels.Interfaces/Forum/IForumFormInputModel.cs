using Forum.MapConfiguration.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Forum.ViewModels.Interfaces.Forum
{
    public interface IForumFormInputModel : IHaveCustomMappings
    {
        IForumInputModel ForumModel { get; set; }

        IEnumerable<SelectListItem> Categories { get; set; }
    }
}