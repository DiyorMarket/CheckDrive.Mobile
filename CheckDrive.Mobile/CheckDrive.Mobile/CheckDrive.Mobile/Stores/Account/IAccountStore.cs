using CheckDrive.Mobile.Models;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Account
{
    public interface IAccountStore
    {
        Task LoginAsync(string login, string password);
        Task LogoutAsync();
        Task<AccountDto> GetAccountAsync();
        Task<AccountDto> UpdateAccountAsync(AccountDto account);
        Task<int> GetEmployeeIdAsync();
        Task<string> GetUserRoleAsync();
        Task<bool> IsLoggedInAsync();
    }
}
