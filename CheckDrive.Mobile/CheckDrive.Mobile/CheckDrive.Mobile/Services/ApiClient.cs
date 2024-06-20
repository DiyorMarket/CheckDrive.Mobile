using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CheckDrive.Mobile.Services
{
    public class ApiClient
    {
        private const string BaseUrl = "https://x60ngf6c-7111.euw.devtunnels.ms/api";
        private readonly HttpClient _client;

        public ApiClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseUrl);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<HttpResponseMessage> GetAsync(string resource, bool isFullUrl = false)
        {
            string url = isFullUrl ? resource : BaseUrl + "/" + resource;

            try
            {
                var token = await SecureStorage.GetAsync("tasty-cookies");

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is empty.");
                }

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_client.BaseAddress, url));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to get data from {resource}. Status code: {response.StatusCode}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string resource, string body)

        {
            try
            {
                string token = SecureStorage.GetAsync("tasty-cookies").Result;

                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/{resource}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(request);

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

        public HttpResponseMessage PutAsync(string url, string data)
        {
            try
            {
                var token = SecureStorage.GetAsync("tasty-cookies").Result;

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is empty.");
                }

                var request = new HttpRequestMessage(HttpMethod.Put, _client.BaseAddress?.AbsolutePath + "/" + url)
                {
                    Content = new StringContent(data, Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = _client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to put data to {url}. Status code: {response.StatusCode}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        public HttpResponseMessage DeleteAsync(string url)
        {
            try
            {
                var token = SecureStorage.GetAsync("tasty-cookies").Result;

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is empty.");
                }

                var request = new HttpRequestMessage(HttpMethod.Delete, _client.BaseAddress?.AbsolutePath + "/" + url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = _client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error fetching url: {url}. Status code: {response.StatusCode}");
                }
                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
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
