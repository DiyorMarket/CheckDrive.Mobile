using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class NotificationViewModel : BaseViewModel
    {
        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public ICommand AcceptCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ObservableCollection<MessageDto> Chats { get; set; }

        public NotificationViewModel(string message)
        {
            Message = message;
            AcceptCommand = new Command(async () => await OnAccept());
            CancelCommand = new Command(async () => await OnCancel());
        }

        public async Task OnAccept()
        {
            await ClosePopup();
            await _navigationService.GoBackAsync();

            Console.WriteLine("Accept button clicked");
        }

        public async Task OnCancel()
        {
            await ClosePopup();
            await _navigationService.GoBackAsync();

            Console.WriteLine("Cancel button clicked");
        }

        private async Task ClosePopup()
        {
            try
            {
                if (PopupNavigation.Instance.PopupStack.Any())
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    Console.WriteLine("Popup yopildi.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Popup yopishda xatolik: {ex.Message}");
                throw new Exception($"Popup yopilmayapti... {ex.Message}");
            }
        }
    }
}
