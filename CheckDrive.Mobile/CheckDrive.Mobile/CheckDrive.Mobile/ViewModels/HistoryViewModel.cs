using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Stores.Drivers;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CheckDrive.Mobile.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly IDriverDataStore _driverDataStore;
        private  int _accountId ;

        public ObservableCollection<History> Reviews { get; private set; }

        public HistoryViewModel(IDriverDataStore driverDataStore)
        {
            _driverDataStore = driverDataStore;

            Reviews = new ObservableCollection<History>();

            LoadViewPage();
        }

        private async Task GetAccountId()
        {
            var token = await SecureStorage.GetAsync("tasty-cookies");

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var accountId = int.Parse(jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            _accountId = accountId;
        }

        public async void LoadViewPage()
        {

            IsBusy = true;

            await GetAccountId();
            await ShowHistory();

            IsBusy = false;
        }

        private async Task ShowHistory()
        {
            var driverHistories = await _driverDataStore.GetDriverHistoryDtosAsync(_accountId);

            if(driverHistories != null)
            {
                foreach (var driverHistory in driverHistories)
                {
                    Reviews.Add(new History()
                    {
                        Date = driverHistory.Date,
                        IsHealthy = driverHistory.IsHealthy,
                        IsHanded = driverHistory.IsHanded,
                        IsGiven = driverHistory.IsGiven,
                        IsAccepted = driverHistory.IsAccepted,
                    });
                }
            }
        }
    }
}
