using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Core.Contracts;
using Kakemons.Core.ViewModels.Home;
using Kakemons.Core.ViewModels.Register;
using Kakemons.SDK.ApiContracts;
using Kakemons.SDK.ApiServiceContracts;
using ReactiveUI;
using Serilog;
using Splat;
using ILogger = Serilog.ILogger;

namespace Kakemons.Core.ViewModels.Login
{
    public class LoginViewModel:BaseViewModel
    {
        readonly IAccountApiService _accountApiService;
        readonly IDialogService _dialogService;
        readonly IAppUserModelService _appUserModelService;
        readonly IUserApiService _userApiService;
        readonly Serilog.ILogger _logger;

        private bool _isUsernameValid;
        private bool _isPasswordValid;
        private ObservableAsPropertyHelper<bool> _canSubmitPropertyHelper;
        private string _username;
        private string _password;

        public LoginViewModel(
            IScreen hostScreen = null,
            IAccountApiService accountApiService = null,
            IDialogService dialogService = null,
            IAppUserModelService appUserModelService = null,
            IUserApiService userApiService = null,
            ILogger logger = null)
        {
            _accountApiService = accountApiService ?? Locator.Current.GetService<IAccountApiService>();
            _dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            _appUserModelService = appUserModelService ?? Locator.Current.GetService<IAppUserModelService>();
            _userApiService = userApiService ?? Locator.Current.GetService<IUserApiService>();
            _logger = logger ?? Locator.Current.GetService<ILogger>();

            GoToRegisterCommand = ReactiveCommand.CreateFromTask(GoToRegister);

            var validateLoginDetailsObservable = this.WhenAnyValue(vm => vm.Username, vm => vm.Password,
                (un, pw) => SetIsUsernameValid(un) && SetIsPasswordValid(pw)).StartWith(false);

            validateLoginDetailsObservable.ToProperty(this, vm => vm.CanSubmit, out _canSubmitPropertyHelper);

            LoginCommand = ReactiveCommand.CreateFromTask(Login, validateLoginDetailsObservable);
        }

        public bool CanSubmit => _canSubmitPropertyHelper.Value;

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public async Task Login()
        {
            try
            {
                var response = await _accountApiService.LoginUser(Username, Password);
                if (response.IsSuccessful)
                {
                    var user = await _userApiService.GetUser();
                    await _appUserModelService.LogInUser(user, response.AccessToken);
                    await HostScreen.Router.Navigate.Execute(new RootViewModel(_logger));
                }
                else
                {
                    Password = "";
                    await _dialogService.AlertAsync("todo", "todo");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(nameof(Login), ex);
                await _dialogService.AlertAsync("todo", "todo");
            }
        }

        public ReactiveCommand<Unit, Unit> GoToRegisterCommand { get; set; }

        public async Task GoToRegister()
        {
            await HostScreen.Router.Navigate.Execute(new RegisterUserViewModel(logger:_logger, dialogService:_dialogService));
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }

        public bool IsUsernameValid
        {
            get => _isUsernameValid;
            set => this.RaiseAndSetIfChanged(ref _isUsernameValid, value);
        }

        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set => this.RaiseAndSetIfChanged(ref _isPasswordValid, value);
        }

        private bool SetIsUsernameValid(string input)
        {
            var isValid = !string.IsNullOrEmpty(input) && input.Length > 3;
            IsUsernameValid = isValid;
            return isValid;
        }

        private bool SetIsPasswordValid(string input)
        {
            var isValid = !string.IsNullOrEmpty(input) && input.Length > 3;
            IsPasswordValid = isValid;
            return isValid;
        }
    }
}
