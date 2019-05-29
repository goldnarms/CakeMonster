using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Kakemons.Common.Parameters;
using Kakemons.Common.Responses;
using Kakemons.Core.Contracts;
using Kakemons.Core.NavigationModels;
using Kakemons.Core.ViewModels.Home;
using Kakemons.Core.ViewModels.Login;
using Kakemons.SDK.ApiContracts;
using ReactiveUI;
using Splat;

namespace Kakemons.Core.ViewModels.Register
{
    public class DisclaimerViewModel: BaseViewModel
    {
        private readonly IScreen _hostScreen;
        private readonly IAppUserModelService _appUserModelService;
        private readonly IUserApiService _userApiService;
        private readonly Serilog.ILogger _logger;
        private GeoCodeResult _address;
        private string _accessToken;
        private string _firstname;
        private string _lastname;
        private string _userId;

        public DisclaimerViewModel(
            DisclaimerNavigation parameter,
            IScreen hostScreen = null,
            IAppUserModelService appUserModelService = null,
            IUserApiService userApiService = null,
            Serilog.ILogger logger = null):base(hostScreen)
        {
            _hostScreen = hostScreen;
            _appUserModelService = appUserModelService ?? Locator.Current.GetService<IAppUserModelService>();
            _userApiService = userApiService ?? Locator.Current.GetService<IUserApiService>();
            _logger = logger ?? Locator.Current.GetService<Serilog.ILogger>();

            AcceptCommand = ReactiveCommand.CreateFromTask(Accept);
            RejectCommand = ReactiveCommand.CreateFromTask(Reject);

            Prepare(parameter);
        }

        public ReactiveCommand<Unit, Unit> RejectCommand { get; set; }

        private async Task Reject()
        {
            await HostScreen.Router.Navigate.Execute(new LoginViewModel(_hostScreen, appUserModelService: _appUserModelService, userApiService: _userApiService, logger: _logger));
        }

        public ReactiveCommand<Unit, Unit> AcceptCommand { get; set; }

        private async Task Accept()
        {
            var userRegistration = new RegisterUserParameter(
                _firstname,
                _lastname,
                _address
            );

            try
            {
                var result = await _userApiService.RegisterUser(userRegistration);
                if (result.IsSuccessful)
                {
                    _appUserModelService.LogInUser(result.User);
                    await HostScreen.Router.Navigate.Execute(new HomeViewModel(_hostScreen, appUserModelService: _appUserModelService, logger: _logger));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(nameof(Accept), ex);
            }
        }

        public void Prepare(DisclaimerNavigation parameter)
        {
            _address = parameter.AddressResult;
            _accessToken = parameter.AccessToken;
            _firstname = parameter.Firstname;
            _lastname = parameter.Lastname;
            _userId = parameter.UserId;
        }
    }
}
