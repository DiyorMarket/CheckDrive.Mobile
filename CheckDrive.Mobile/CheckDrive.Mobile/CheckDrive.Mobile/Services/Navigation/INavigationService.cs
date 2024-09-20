using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.ViewModels;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToAsync(NavigationPageType pageType);
        Task GoBackAsync();
    }
}
