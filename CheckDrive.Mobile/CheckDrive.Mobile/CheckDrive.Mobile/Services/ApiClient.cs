using CheckDrive.Mobile.Exceptions;
using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Services
{
    public class ApiClient
    {
        private const string BaseUrl = "https://4hq2t8p1-7111.euw.devtunnels.ms/api/";
        private readonly HttpClient _client;

        public ApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl),
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

        public async Task<TResult> GetAsync<TResult>(string resource)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_client.BaseAddress, resource));
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResult>(json);

            return result;
        }

        public async Task<HttpResponseMessage> PostAsync<TBody>(string resource, TBody body)
        {
            var json = JsonConvert.SerializeObject(body);
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(_client.BaseAddress, resource))
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);

            return response;
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string resource, TRequest body)
        {
            try
            {
                string token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);

                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidTokenException("Token is empty.");
                }

                var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/{resource}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(request).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResponse>(responseBody);

                return result;
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
