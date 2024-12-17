using CheckDrive.Mobile.Models.Dispatcher;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Dispatcher
{
    public interface IDispatcherStore
    {
        Task CreateReveiwAsync(DispatcherReviewRequest request);
        Task<List<DispatcherHistory>> GetHistoryAsync();
    }
}
