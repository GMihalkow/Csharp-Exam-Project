namespace Forum.Web.Services.Contracts
{
    using Forum.Models;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IAccountService
    {
        int GetUsersCount();

        ForumUser GetUser(ClaimsPrincipal principal);

        void LoginUser(ForumUser model, string password);

        void LogoutUser();

        string GetNewestUser();

        Task<bool> EmailExists(string email);

        bool UserExists(string username);

        Task<bool> UserWithPasswordExists(string username, string password);

        void RegisterUser(ForumUser model, string password);
    }
}