﻿using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Mobile.Responses;
using CheckDrive.Mobile.Services;
using CheckDrive.Web.Stores.OperatorReviews;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.OperatorReviews
{
    public class OperatorReviewDataStore : IOperatorReviewDataStore
    {
        private readonly ApiClient _api;

        public OperatorReviewDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetOperatorReviewResponse> GetOperatorReviewsAsync()
        {
            var response = await _api.GetAsync("operators/reviews");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch operator reviews.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetOperatorReviewResponse>(json);

            return result;
        }

        public async Task<GetOperatorReviewResponse> GetOperatorReviewsAsync(DateTime date)
        {
            StringBuilder query = new StringBuilder("");

            if (date != DateTime.MinValue)
            {
                query.Append($"Date={date.Date}&");
            }

            var response = await _api.GetAsync("operators/reviews?" + query.ToString());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch operator reviews.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetOperatorReviewResponse>(json);

            return result;
        }

        public async Task<GetOperatorReviewResponse> GetOperatorReviewsByDriverIdAsync(int driverId)
        {
            StringBuilder query = new StringBuilder("");

            if (!driverId.Equals(0))
            {
                query.Append($"driverId={driverId}");
            }

            var response = await _api.GetAsync("operators/reviews?" + query.ToString());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch operator reviews.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetOperatorReviewResponse>(json);

            return result;
        }

        public async Task<OperatorReviewDto> GetOperatorReviewAsync(int id)
        {
            var response = await _api.GetAsync($"operators/review/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch operator reviews with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperatorReviewDto>(json);

            return result;
        }

        public async Task<OperatorReviewDto> CreateOperatorReviewAsync(OperatorReviewForCreateDto operatorReview)
        {
            var json = JsonConvert.SerializeObject(operatorReview);
            var response = await _api.PostAsync("operators/review", json);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating operator reviews.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OperatorReviewDto>(jsonResponse);
        }
    }
}