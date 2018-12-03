using Forum.MapConfiguration.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Forum.ViewModels.Interfaces.Forum
{
    public interface IForumFormInputModel : IHaveCustomMappings
    {
        IForumInputModel ForumModel { get; set; }

        SelectListItem[] Categories { get; set; }
    }
}