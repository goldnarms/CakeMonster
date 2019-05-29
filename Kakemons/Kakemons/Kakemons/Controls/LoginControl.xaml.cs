using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.CreateUser;
using Kakemons.Core.ViewModels.Login;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginControl : ContentView, IDisposable
    {
        private CompositeDisposable _cd;
        private readonly IScreen _hostScreen;

        public LoginControl(IScreen hostScreen)
        {

            InitializeComponent();
            _cd = new CompositeDisposable();

            GoToCreateUserCommand = ReactiveCommand.CreateFromTask(GoToCreateUser)
                .DisposeWith(_cd);

            GoToLoginCommand = ReactiveCommand.CreateFromTask(GoToLogin)
                .DisposeWith(_cd);
            _hostScreen = hostScreen;
        }

        private async Task GoToCreateUser()
        {
            await _hostScreen.Router.Navigate.Execute(new CreateUserViewModel());
        }

        private async Task GoToLogin()
        {
            await _hostScreen.Router.Navigate.Execute(new LoginViewModel(_hostScreen));
        }

        public ReactiveCommand<Unit, Unit> GoToCreateUserCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToLoginCommand { get; }

        public void Dispose()
        {
            _cd?.Clear();
            GoToCreateUserCommand?.Dispose();
            GoToLoginCommand?.Dispose();
        }
    }
}
