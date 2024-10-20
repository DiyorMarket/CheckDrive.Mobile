using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Auth
{
    public interface IAuthStore
    {
        Task<bool> LoginAsync(string username, string password);
        void Logout();
        Task<bool> IsLoggedInAsync();
    }
}
