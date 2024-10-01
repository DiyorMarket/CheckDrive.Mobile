using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IReadOnlyDictionary<NavigationPageType, string> _pages;

        public NavigationService()
        {
            _pages = new Dictionary<NavigationPageType, string>()
            {
                { NavigationPageType.Login, nameof(LoginPage) },
                { NavigationPageType.Profile, nameof(ProfilePage) },
                { NavigationPageType.EditProfile, nameof(EditProfile) },
                { NavigationPageType.Home, nameof(HomePage) },
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
    }
}
