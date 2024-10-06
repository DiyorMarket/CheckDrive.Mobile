using System;

namespace CheckDrive.Mobile.Exceptions
{
    public class InvalidAccountException : Exception
    {
        public InvalidAccountException() { }
        public InvalidAccountException(string message) : base(message) { }
    }
}
