using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.CheckPoint
{
    public class MockCheckPointStore : ICheckPointStore
    {
        public async Task<List<CheckPointDto>> GetCheckPointsAsync(CheckPointStage stage)
        {
            var checkPointsCount = new Random().Next(5, 20);

            List<CheckPointDto> checkPoints = new List<CheckPointDto>();
            for (int i = 0; i < checkPointsCount; i++)
            {
                checkPoints.Add(FakeDataGenerator.GetCheckPoint());
            }

            await Task.Delay(1250);

            return checkPoints;
        }
    }
}
