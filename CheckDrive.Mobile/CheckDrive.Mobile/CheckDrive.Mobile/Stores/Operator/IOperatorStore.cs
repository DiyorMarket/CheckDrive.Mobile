using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Operator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Operator
{
    public interface IOperatorStore
    {
        Task<List<OilMark>> GetOilMarksAsync();
        Task<List<OperatorHistoryDto>> GetHistoriesAsync();
        Task CreateReviewAsync(OperatorReviewRequest request);
    }
}
