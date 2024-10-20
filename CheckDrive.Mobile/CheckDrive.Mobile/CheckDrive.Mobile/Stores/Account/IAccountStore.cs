using CheckDrive.Mobile.Models.Account;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Account
{
    public interface IAccountStore
    {
        Task<AccountDto> GetAccountAsync(string accountId);
        Task<AccountDto> UpdateAccountAsync(AccountDto account);
        Task<string> GetAccountIdAsync();
        Task<int> GetUserIdAsync();
        Task<string> GetUserRoleAsync();
    }
}
