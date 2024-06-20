using CheckDrive.ApiContracts.Driver;
using Newtonsoft.Json;
using System;
using Xamarin.Essentials;

namespace CheckDrive.Mobile.Services
{
    public static class DataService
    {
        private const string securetyKey = "accountData";
        private const string securetyKeySavedDate = "savedDate";
        private const string securetyKeyToken = "tasty-cookies";

        public static void SaveAccount(DriverDto account)
        {
            try
            {
                var jsonDateTime = JsonConvert.SerializeObject(DateTime.Now);
                var json = JsonConvert.SerializeObject(account);
                SecureStorage.SetAsync(securetyKeySavedDate, jsonDateTime);
                SecureStorage.SetAsync(securetyKey, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving {securetyKey}: {ex.Message}");
            }
        }

        public static void SaveToken(string token)
        {
            try
            {
            SecureStorage.SetAsync(securetyKeyToken, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving token. {ex.Message}");
            }
        }
        public static DriverDto GetAccount()
        {
            try
            {
                if (SecureStorage.GetAsync(securetyKey).GetAwaiter().GetResult() != null)
                {
                    var json = SecureStorage.GetAsync(securetyKey).GetAwaiter().GetResult();
                    return JsonConvert.DeserializeObject<DriverDto>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting {securetyKey}: {ex.Message}");
            }
            return new DriverDto();
        }

        public static DateTime GetCreationDate()
        {
            try
            {
                if (SecureStorage.GetAsync(securetyKeySavedDate).GetAwaiter().GetResult() != null)
                {
                    var json = SecureStorage.GetAsync(securetyKeySavedDate).GetAwaiter().GetResult();
                    return JsonConvert.DeserializeObject<DateTime>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting {securetyKeySavedDate}: {ex.Message}");
            }
            return new DateTime();
        }

        public static void RemoveAcoountData()
        {
            try
            {
                if (SecureStorage.GetAsync(securetyKey).GetAwaiter().GetResult() != null 
                    && SecureStorage.GetAsync(securetyKeySavedDate).GetAwaiter().GetResult() != null)
                {
                    SecureStorage.Remove(securetyKey);
                    SecureStorage.Remove(securetyKeySavedDate);
                    SecureStorage.Remove(securetyKeyToken);
                    Console.WriteLine("file successfuly deleted");
                    return;
                }

                Console.WriteLine("file with this name cannot fined");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting: {ex.Message}");
            }
        }
    }
}
