using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Views;
using CheckDrive.Mobile.Views.Errors;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IReadOnlyDictionary<NavigationPageType, string> _pages;
        private readonly IReadOnlyDictionary<NavigationPageType, Func<string, PopupPage>> _popups;

        public NavigationService()
        {
            _pages = new Dictionary<NavigationPageType, string>()
            {
                { NavigationPageType.Login, nameof(LoginPage) },
                { NavigationPageType.Profile, nameof(ProfilePage) },
                { NavigationPageType.Home, nameof(HomePage) },
            };

            _popups = new Dictionary<NavigationPageType, Func<string, PopupPage>>()
            {
                { NavigationPageType.NotificationPopup, (message) => new NotificationPopupPage(message) },
            };
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public async Task NavigateToAsync(NavigationPageType pageType)
        {
            if (!_pages.TryGetValue(pageType, out var page))
            {
                throw new InvalidOperationException($"Could not find navigation for page: {pageType}");
            }

            await Shell.Current.GoToAsync($"//{page}");
        }

        public async Task NavigateToAsync(NavigationPageType pageType, string message)
        {
            if (_popups.TryGetValue(pageType, out var createPopup))
            {
                await ShowPopupAsync(createPopup(message));
            }
            else if (_pages.TryGetValue(pageType, out var page))
            {
                await Shell.Current.GoToAsync($"//{page}");
            }
            else
            {
                throw new InvalidOperationException($"Could not find navigation for page: {pageType}");
            }
        }

        private async Task ShowPopupAsync(PopupPage popupPage)
        {
            if (popupPage == null)
            {
                throw new ArgumentNullException(nameof(popupPage), "Popup cannot be null.");
            }
            await PopupNavigation.Instance.PushAsync(popupPage);
        }
    }
}
