using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Accounts
{
    public interface IAccountStore
    {
        Task LoginAsync(string login, string password);
        Task LogoutAsync();

    }
}
