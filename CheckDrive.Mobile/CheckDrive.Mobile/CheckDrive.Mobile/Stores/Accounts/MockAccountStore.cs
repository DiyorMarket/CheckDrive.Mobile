using CheckDrive.Mobile.Models;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Accounts
{
    internal class MockAccountStore : IAccountStore
    {
        public Task<Account> GetAccountAsync()
        {
            var account = new Account
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
