using CheckDrive.Mobile.Models.Enums;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateToAsync(NavigationPageType pageType);
        Task GoBackAsync();
    }
}
