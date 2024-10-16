﻿using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.History
{
    public class MockHistoryStore : IHistoryStore
    {
        private static Random _random = new Random();

        public async Task<List<CheckPointHistoryDto>> GetHistoriesAsync()
        {
            await Task.Delay(1000);
            List<CheckPointHistoryDto> histories = new List<CheckPointHistoryDto>();

            for (int i = 0; i < _random.Next(4, 20); i++)
            {
                var history = FakeDataGenerator.GetCheckPointHistory();
                histories.Add(history);
            }

            return histories;
        }
    }
}
