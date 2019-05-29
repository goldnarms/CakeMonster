using System;
using System.Reactive;
using System.Threading.Tasks;
using Kakemons.Common.Contracts;
using Kakemons.Common.Parameters;
using Kakemons.Common.Responses;
using Kakemons.Core.Contracts;
using Kakemons.Core.NavigationModels;
using Kakemons.Core.ViewModels.Home;
using Kakemons.Core.ViewModels.Login;
using Kakemons.SDK.ApiContracts;
using ReactiveUI;

namespace Kakemons.Core.ViewModels.Register
{
    public class DisclaimerViewModel: BaseViewModel
    {
        private readonly IAppUserModelService _appUserModelService;
        private readonly IUserApiService _userApiService;
        private readonly ILogger _logger;
        private GeoCodeResult _address;
        private string _accessToken;
        private string _firstname;
        private string _lastname;
        private string _userId;

        public DisclaimerViewModel(
            DisclaimerNavigation parameter
            IAppUserModelService appUserModelService,
            IUserApiService userApiService,
            ILogger logger)
        {
            _appUserModelService = appUserModelService;
            _userApiService = userApiService;
            _navigationService = navigationService;
            _logger = logger;

            AcceptCommand = ReactiveCommand.CreateFromTask(Accept);
            RejectCommand = ReactiveCommand.CreateFromTask(Reject);
        }

        public ReactiveCommand<Unit, Unit> RejectCommand { get; set; }

        private async Task Reject()
        {
            await _navigationService.Navigate<LoginViewModel>();
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
                    await _navigationService.Navigate<HomeViewModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(Accept), ex);
            }
        }

        public override void Prepare(DisclaimerNavigation parameter)
        {
            _address = parameter.AddressResult;
            _accessToken = parameter.AccessToken;
            _firstname = parameter.Firstname;
            _lastname = parameter.Lastname;
            _userId = parameter.UserId;
        }
    }
}
