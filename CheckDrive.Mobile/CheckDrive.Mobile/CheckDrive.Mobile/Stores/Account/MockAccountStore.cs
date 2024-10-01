using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Account
{
    internal class MockAccountStore : IAccountStore
    {
        private AccountDto _account;

        public MockAccountStore()
        {
            _account = new AccountDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Passport = "1234567891987654",
                PhoneNumber = "+998 88 345 67 89",
                Login = "johndoe",
                Address = "123 Main St, Cityville",
                Birthdate = new System.DateTime(1990, 1, 1),
            };
        }

        public Task<AccountDto> GetAccountAsync()
        {
            return Task.FromResult(_account);
        }

        public Task<AccountDto> UpdateAccountAsync(AccountDto account)
        {
            _account = account;

            return Task.FromResult(_account);
        }

        public Task LoginAsync(string login, string password)
        {
            return Task.CompletedTask;
        }

        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }
    }
}
