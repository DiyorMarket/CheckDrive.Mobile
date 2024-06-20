using CheckDrive.ApiContracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.Accounts
{
    public class MockAccountDataStore 
    {
        private readonly List<AccountDto> _accounts;

        public MockAccountDataStore()
        {
            _accounts = new List<AccountDto>
            {
                new AccountDto { Id = 1, Login = "user1", Password = "password1", PhoneNumber = "123456789", FirstName = "John", LastName = "Doe", Bithdate = new DateTime(1990, 1, 1), RoleName = "Driver" },
                new AccountDto { Id = 2, Login = "user2", Password = "password2", PhoneNumber = "987654321", FirstName = "Jane", LastName = "Siu", Bithdate = new DateTime(1995, 5, 15), RoleName = "Operator" },
            };
        }

        public async Task<List<AccountDto>> GetAccounts()
        {
            await Task.Delay(100);
            return _accounts.ToList();
        }

        public async Task<AccountDto> GetAccount(int id)
        {
            await Task.Delay(100);
            return _accounts.FirstOrDefault(a => a.Id == id);
        }

        public async Task<AccountDto> CreateAccount(AccountDto account)
        {
            await Task.Delay(100);
            account.Id = _accounts.Max(a => a.Id) + 1;
            _accounts.Add(account);
            return account;
        }

        public async Task<AccountDto> UpdateAccount(int id, AccountDto account)
        {
            await Task.Delay(100);
            var existingAccount = _accounts.FirstOrDefault(a => a.Id == id);
            if (existingAccount != null)
            {
                existingAccount.Login = account.Login;
                existingAccount.Password = account.Password;
                existingAccount.PhoneNumber = account.PhoneNumber;
                existingAccount.FirstName = account.FirstName;
                existingAccount.LastName = account.LastName;
                existingAccount.Bithdate = account.Bithdate;
                existingAccount.RoleName = account.RoleName;
            }
            return existingAccount;
        }

        public async Task DeleteAccount(int id)
        {
            await Task.Delay(100);
            var accountToRemove = _accounts.FirstOrDefault(a => a.Id == id);
            if (accountToRemove != null)
            {
                _accounts.Remove(accountToRemove);
            }
        }
    }
}
