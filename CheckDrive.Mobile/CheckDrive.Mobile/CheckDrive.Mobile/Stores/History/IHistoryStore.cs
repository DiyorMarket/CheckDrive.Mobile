using System.Collections.Generic;
using System.Threading.Tasks;
using CheckDrive.Mobile.Models;

namespace CheckDrive.Mobile.Stores.History
{
    public interface IHistoryStore
    {
        Task<List<HistoryDto>> GetHistoriesAsync();
    }
}
