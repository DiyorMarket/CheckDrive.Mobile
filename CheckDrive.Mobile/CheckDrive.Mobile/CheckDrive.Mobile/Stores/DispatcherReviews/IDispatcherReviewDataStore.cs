using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Mobile.Responses;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public interface IDispatcherReviewDataStore
    {
        Task<GetDispatcherReviewResponse> GetDispatcherReviewsAsync();
        Task<DispatcherReviewDto> GetDispatcherReviewAsync(int id);
        Task<DispatcherReviewDto> CreateDispatcherReviewAsync(DispatcherReviewForCreateDto dispatcherReview);
    }
}
