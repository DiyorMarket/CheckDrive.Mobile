using CheckDrive.Mobile.Exceptions;
using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CheckDrive.Mobile.Services
{
    public class ApiClient
    {
        private const string BaseUrl = "https://4hq2t8p1-7111.euw.devtunnels.ms/api";
        private readonly HttpClient _client;

        public ApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<HttpResponseMessage> GetAsync(string resource, bool isFullUrl = false)
        {
            string url = isFullUrl ? resource : BaseUrl + "/" + resource;

            try
            {
                var token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);

                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidTokenException("Token is empty.");
                }

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_client.BaseAddress, url));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _client.SendAsync(request).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to get data from {resource}. Status code: {response.StatusCode}");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error making api request: {ex.Message}.");
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string resource, string body)
        {
            try
            {
                string token = await SecureStorage.GetAsync("tasty-cookies");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/{resource}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(request).ConfigureAwait(false);

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
