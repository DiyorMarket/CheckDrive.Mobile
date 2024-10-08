using System;

namespace CheckDrive.Mobile.Models
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public int EmployeeId { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
