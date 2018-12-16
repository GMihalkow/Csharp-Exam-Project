using Forum.MapConfiguration.Contracts;
using Forum.Models;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels.Interfaces.Forum
{
    public interface IForumInputModel : IValidatableObject
    {
        string Name { get; set; }

        string Description { get; set; }

        string Category { get; set; }
    }
}