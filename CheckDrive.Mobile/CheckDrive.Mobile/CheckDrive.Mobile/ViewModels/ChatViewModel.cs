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
        public ICommand AcceptCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ObservableCollection<ChatDto> Chats { get; set; }

        public ChatViewModel()
        {
            AcceptCommand = new Command(async () => await OnAccept());
            CancelCommand = new Command(async () => await OnCancel());

            Chats = new ObservableCollection<ChatDto>
            {
                new ChatDto { HeadlineText = "Yoqilgi quyish", Title = "Hello from Chat 1" },
                new ChatDto { HeadlineText = "Avtomobil qabul qilish", Title = "Welcome to Chat 2" },
                new ChatDto { HeadlineText = "Shifikor korigi", Title = "Let's discuss in Chat 3" }
            };
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
