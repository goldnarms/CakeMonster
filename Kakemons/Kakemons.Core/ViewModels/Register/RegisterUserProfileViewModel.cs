using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Kakemons.Core.NavigationModels;
using Kakemons.Core.ViewModels.Login;
using ReactiveUI;
using Splat;
using ILogger = Kakemons.Common.Contracts.ILogger;

namespace Kakemons.Core.ViewModels.Register
{
    public class RegisterUserProfileViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        private readonly IScreen _hostScreen;
        private string _firstname;
        private string _lastname;

        public RegisterUserProfileViewModel(UserProfileNavigation userProfile, IScreen hostScreen = null, ILogger logger = null):base(hostScreen)
        {
            _logger = logger;
            _hostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
            _logger = logger ?? Locator.Current.GetService<ILogger>();
            Prepare(userProfile);
            CancelCommand = ReactiveCommand.CreateFromTask(Cancel);
            GoToLoginUserCommand = ReactiveCommand.CreateFromTask(GoToLoginUser);
            GoToSetAddressCommand = ReactiveCommand.CreateFromTask(SetAddress);
        }

        public string Firstname
        {
            get => _firstname;
            set => this.RaiseAndSetIfChanged(ref _firstname, value);
        }

        public string Lastname
        {
            get => _lastname;
            set => this.RaiseAndSetIfChanged(ref _lastname, value);
        }


        public ReactiveCommand<Unit, Unit> GoToSetAddressCommand { get; set; }

        private async Task SetAddress()
        {
            var setAddressNavigation = new SetAddressNavigation(UserId, AccessToken, Firstname, Lastname);
            await HostScreen.Router.Navigate.Execute(new RegisterSetAddressViewModel(setAddressNavigation, _hostScreen));
        }

        public ReactiveCommand<Unit, Unit> GoToLoginUserCommand { get; set; }

        private async Task GoToLoginUser()
        {
            await _navigationService.Navigate<LoginViewModel>();
        }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

        private async Task Cancel()
        {
            await _navigationService.Navigate<LoginViewModel>();
        }

        public void Prepare(UserProfileNavigation parameter)
        {
            UserId = parameter.UserId;
            AccessToken = parameter.AccessToken;
        }

        public string AccessToken { get; private set; }

        public string UserId { get; private set; }
    }
}
