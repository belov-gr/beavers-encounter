using Beavers.Encounter.Core;

namespace Beavers.Encounter.ApplicationServices
{
    public interface IUserService
    {
        User CreateNewUser();
        User CurrentUser { get; }
        void SetAuthenticationCookie(string login);
        void SetContextUserTo(User user);
        void RemoveAuthenticationCookie();
        string HashPassword(string password);
        bool Authenticate(string login, string password);
    }
}
