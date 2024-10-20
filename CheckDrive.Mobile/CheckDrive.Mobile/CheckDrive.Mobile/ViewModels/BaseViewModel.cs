using CheckDrive.Mobile.Services.Navigation;
using CheckDrive.Mobile.Views.Popup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly INavigationService _navigationService;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }

        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected BaseViewModel()
        {
            _navigationService = DependencyService.Get<INavigationService>();
        }

        protected static async Task DisplayErrorAsync(string message, string details)
        {
            message += " Iltimios, qayta urunib ko'ring yoki texnik yordam bilan bog'laning.";
            var popup = new ErrorPopup(message, details);

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        protected async Task DisplaySuccessAsync(string message)
        {
            var popup = new SuccessPopup(message);

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            Action onChanged = null,
            [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
