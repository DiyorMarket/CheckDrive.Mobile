using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Operator
{
    public interface IOperatorStore
    {
        Task<List<OilMark>> GetOilMarksAsync();
        Task CreateReviewAsync(OperatorReview review);
    }
}
