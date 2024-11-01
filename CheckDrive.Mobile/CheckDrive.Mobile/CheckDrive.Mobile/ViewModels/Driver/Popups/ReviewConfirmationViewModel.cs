using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Driver.Popups
{
    public class ReviewConfirmationViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<bool> _completionSource;

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand AcceptCommand { get; }
        public ICommand RejectCommand { get; }


        public ReviewConfirmationViewModel(string message, TaskCompletionSource<bool> completionSource)
        {
            Message = message;
            _completionSource = completionSource;

            AcceptCommand = new Command(OnAccept);
            RejectCommand = new Command(OnReject);
        }

        public void OnAccept()
        {
            _completionSource.SetResult(true);
        }

        public void OnReject()
        {
            _completionSource.SetResult(false);
        }
    }
}
