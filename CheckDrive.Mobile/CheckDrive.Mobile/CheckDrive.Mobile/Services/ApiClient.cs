﻿using CheckDrive.Mobile.Responses;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Services
{
    public class ApiClient
    {
        private const string BaseUrl = "https://jsw763dc-7111.euw.devtunnels.ms/api";

        private readonly HttpClient _client;

        public ApiClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseUrl);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        public async Task<ApiResponse<T>> GetAsync<T>(string resource, bool isFullUrl = false)
        {
            string url = isFullUrl ?
                resource :
                BaseUrl + "/" + resource;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to get data from {resource}. Status code: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse<T>>(json)
                       ?? throw new JsonSerializationException();
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