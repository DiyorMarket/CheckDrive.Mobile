using CheckDrive.Mobile.Models;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Account
{
    internal class MockAccountStore : IAccountStore
    {
        public Task<AccountDto> GetAccountAsync()
        {
            var account = new AccountDto
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Address = "Tashkent, Uzbekistan",
                Birthdate = DateTime.Now.AddYears(-40),
                Passport = "FS231 23B"
            };

            return Task.FromResult(account);
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
