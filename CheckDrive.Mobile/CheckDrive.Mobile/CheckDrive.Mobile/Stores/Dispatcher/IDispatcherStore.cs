using CheckDrive.Mobile.Models.Review;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Dispatcher
{
    public interface IDispatcherStore
    {
        Task CreateReveiwAsync(DispatcherReview review);
    }
}
