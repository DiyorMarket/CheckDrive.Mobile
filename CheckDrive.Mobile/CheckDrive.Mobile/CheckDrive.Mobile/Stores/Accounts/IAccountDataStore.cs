using CheckDrive.ApiContracts.Account;
using CheckDrive.Mobile.Responses;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.Accounts
{
    public interface IAccountDataStore
    {
        Task<string> CreateTokenAsync(string login, string password);
        Task<GetAccountResponse> GetAccountsAsync(string login);
        Task<AccountDto> GetAccountAsync(int id);
        Task<AccountDto> CreateAccountAsync(AccountDto account);
    }
}
