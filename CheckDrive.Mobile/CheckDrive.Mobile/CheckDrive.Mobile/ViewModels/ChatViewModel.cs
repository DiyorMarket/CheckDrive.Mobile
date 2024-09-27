using CheckDrive.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class ChatViewModel : BaseViewModel
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

        public ObservableCollection<ChatDto> Chats { get; set; }

        public ChatViewModel()
        {
            AcceptCommand = new Command(async () => await OnAccept());
            CancelCommand = new Command(async () => await OnCancel());
        }

        public async Task OnAccept()
        {
            Console.WriteLine("Accept button clicked");
        }

        public async Task OnCancel()
        {
            Console.WriteLine("Cancel button clicked");
        }
    }
}
