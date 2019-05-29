using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Kakemons.Common.Responses;
using Kakemons.Core.Contracts;
using Kakemons.Core.NavigationModels;
using Kakemons.Core.ViewModels.Login;
using Kakemons.SDK.ApiContracts;
using ReactiveUI;
using Splat;
using ILogger = Kakemons.Common.Contracts.ILogger;

namespace Kakemons.Core.ViewModels.Register
{
    public class RegisterSetAddressViewModel:BaseViewModel
    {
        private readonly IScreen _hostScreen;
        private readonly IGeoLocationService _geoLocationService;
        private readonly IDialogService _dialogService;
        private readonly ILogger _logger;
        private string _accessToken;
        private string _firstname;
        private string _lastname;
        private string _userId;
        private string _addressSearchText;
        private ObservableAsPropertyHelper<bool> _canSearchPropertyHelper;
        private GeoCodeResult _selectedGeoResult;

        public RegisterSetAddressViewModel(SetAddressNavigation setAddress, IScreen hostScreen = null, IGeoLocationService geoLocationService = null, IDialogService dialogService = null, ILogger logger = null):base(hostScreen)
        {
            _hostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
            _geoLocationService = geoLocationService ?? Locator.Current.GetService<IGeoLocationService>();
            _dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            _logger = logger ?? Locator.Current.GetService<ILogger>();

            Prepare(setAddress);
            CancelCommand = ReactiveCommand.CreateFromTask(Cancel);
            RegisterCommand = ReactiveCommand.CreateFromTask(Register);

            var validateSearchObservable = this.WhenAnyValue(vm => vm.AddressSearchText,
                (searchText) => searchText.Length > 3).StartWith(false);

            validateSearchObservable.ToProperty(this, vm => vm.CanSearch, out _canSearchPropertyHelper);

            SearchAddressCommand = ReactiveCommand.CreateFromTask(SearchAddress, validateSearchObservable);
        }

        public GeoCodeResult SelectedGeoResult
        {
            get => _selectedGeoResult;
            set => this.RaiseAndSetIfChanged(ref _selectedGeoResult, value);
        }

        private async Task SearchAddress()
        {
            try
            {
                var result = await _geoLocationService.Search(AddressSearchText);
                if (result.Status == "OK" && (result.Results?.Any() ?? false))
                {
                    var resultsText = result.Results.ToDictionary(r => r.PlaceId, r => r.FormattedAddress);
                    await _dialogService.ActionSheetAsync(resultsText);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                await _dialogService.AlertAsync("", "");
            }
        }

        public bool CanSearch => _canSearchPropertyHelper.Value;

        public ReactiveCommand<Unit, Unit> SearchAddressCommand { get; set; }

        public ReactiveCommand<Unit, Unit> RegisterCommand { get; set; }

        public string AddressSearchText
        {
            get => _addressSearchText;
            set => this.RaiseAndSetIfChanged(ref _addressSearchText, value);
        }

        private async Task Register()
        {
            var disclaimerNavigation =
                new DisclaimerNavigation(_userId, _accessToken, _firstname, _lastname, SelectedGeoResult);
            await HostScreen.Router.Navigate.Execute(new DisclaimerViewModel(setAddressNavigation, _hostScreen));
            await _navigationService.Navigate<DisclaimerViewModel, DisclaimerNavigation>(disclaimerNavigation);
        }

        public void Prepare(SetAddressNavigation parameter)
        {
            _accessToken = parameter.AccessToken;
            _firstname = parameter.Firstname;
            _lastname = parameter.Lastname;
            _userId = parameter.UserId;
        }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

        private async Task Cancel()
        {
            await _navigationService.Navigate<LoginViewModel>();
        }
    }
}
