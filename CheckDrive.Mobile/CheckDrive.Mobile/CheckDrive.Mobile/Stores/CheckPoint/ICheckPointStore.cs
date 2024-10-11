using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.CheckPoint
{
    public interface ICheckPointStore
    {
        Task<List<CheckPointDto>> GetCheckPointsAsync(CheckPointStage stage);
    }
}
