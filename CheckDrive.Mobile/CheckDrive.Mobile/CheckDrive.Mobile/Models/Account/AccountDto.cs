using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Account
{
    public class AccountDto
    {
        public int UserId { get; set; }
        public string AccountId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passport { get; set; }
        public string Address { get; set; }
        public DateTime Birthdate { get; set; }
        public EmployeePosition Position { get; set; }
    }
}
