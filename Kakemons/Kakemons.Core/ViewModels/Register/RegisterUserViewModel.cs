using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Kakemons.Common.Contracts;
using Kakemons.Core.Contracts;
using Kakemons.Core.NavigationModels;
using Kakemons.Core.ViewModels.Login;
using ReactiveUI;

namespace Kakemons.Core.ViewModels.Register
{
    public class RegisterUserViewModel : BaseViewModel
    {
        private readonly IExternalService _externalService;
        private readonly IScreen _hostScreen;
        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;

        public RegisterUserViewModel(
            IScreen hostScreen = null,
            IExternalService externalService = null,
            ILogger logger = null,
            IDialogService dialogService = null):base(hostScreen)
        {
            _hostScreen = hostScreen;
            _logger = logger;
            RegisterFacebookCommand = ReactiveCommand.CreateFromTask(RegisterWithFacebook);
            RegisterWithGoolgeCommand = ReactiveCommand.CreateFromTask(RegisterWithGoogle);
            GoToRegisterWithUsernameCommand = ReactiveCommand.CreateFromTask(GoToRegisterWithUsername);
            GoToLoginCommand = ReactiveCommand.CreateFromTask(GoToLogin);
        }

        public ReactiveCommand<Unit, Unit> GoToRegisterWithUsernameCommand { get; set; }

        private async Task GoToRegisterWithUsername()
        {
            await HostScreen.Router.Navigate.Execute(new RegisterUsernameViewModel());
        }

        private async Task RegisterWithGoogle()
        {
            try
            {
                var result = await _externalService.RegisterWithGoogle();
                var userProfileNavigation = new UserProfileNavigation(result.UserId, result.AccessToken);
                await HostScreen.Router.Navigate.Execute(new RegisterUserProfileViewModel(userProfileNavigation, _hostScreen));
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(RegisterWithFacebook), ex);
                await _dialogService.PresentAlertAsync("Tittel", "Body");
            }
        }

        public ReactiveCommand<Unit, Unit> RegisterWithGoolgeCommand { get; set; }

        public ReactiveCommand<Unit, Unit> RegisterFacebookCommand { get; set; }

        public ReactiveCommand<Unit, Unit> GoToLoginCommand { get; set; }

        private async Task RegisterWithFacebook()
        {
            try
            {
                var result = await _externalService.RegisterWithFacebook();
                var userProfileNavigation = new UserProfileNavigation(result.UserId, result.AccessToken);
                await HostScreen.Router.Navigate.Execute(new RegisterUserProfileViewModel(userProfileNavigation, _hostScreen));
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(RegisterWithFacebook), ex);
                await _dialogService.PresentAlertAsync("Tittel", "Body");
            }
        }

        private async Task GoToLogin()
        {
            await HostScreen.Router.Navigate.Execute(new LoginViewModel(_hostScreen, dialogService: _dialogService, logger: _logger));
        }
    }
}
