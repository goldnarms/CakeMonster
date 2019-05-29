using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.Login;
using Kakemons.Core.ViewModels.Register;
using ReactiveUI;
using Splat;

namespace Kakemons.Core.ViewModels.Onboarding
{
    public class OnboardingViewModel : BaseViewModel
    {
        private readonly IScreen _hostScreen;
        private readonly int _pageCount = 3;
        private List<OnboardingCardItem> _onboardingCards;
        public OnboardingViewModel(IScreen hostScreen = null) : base(hostScreen)
        {
            _hostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
            GoToLoginCommand = ReactiveCommand.CreateFromTask(GoToLogin);
            GoToRegisterCommand = ReactiveCommand.CreateFromTask(GoToRegister);
            NextCommand = ReactiveCommand.Create(Next);
            Position = 0;

            _onboardingCards = new List<OnboardingCardItem>
            {
                new OnboardingCardItem()
                {
                    Header = "Finn kaker",
                    IsLastPage = false,
                    Text = "Finn kaker i nærheten"
                },
                new OnboardingCardItem()
                {
                    Header = "Ta med",
                    IsLastPage = false,
                    Text = "Still opp med kaker"
                },
                new OnboardingCardItem()
                {
                    Header = "Registrer deg",
                    IsLastPage = true,
                    Text = "Gå videre for å registere deg"
                }
            };
        }

        public ReactiveCommand<Unit, Unit> NextCommand { get; set; }

        public ReactiveCommand<Unit, Unit> GoToRegisterCommand { get; set; }

        public ReactiveCommand<Unit, Unit> GoToLoginCommand { get; set; }

        public int Position
        {
            get => _position;
            set
            {
                this.RaiseAndSetIfChanged(ref _position, value);
                this.RaisePropertyChanged(nameof(IsOnLastPage));
            }

        }
        private void Next()
        {
            Position = Position + 1;
        }

        public List<OnboardingCardItem> OnboardingCards
        {
            get => _onboardingCards;
            set => this.RaiseAndSetIfChanged(ref _onboardingCards, value);
        }


        private async Task GoToLogin()
        {
            await HostScreen.Router.Navigate.Execute(new LoginViewModel(_hostScreen));
        }

        private async Task GoToRegister()
        {
            await HostScreen.Router.Navigate.Execute(new RegisterUserViewModel(_hostScreen));
        }

        private int _position;
        public bool IsOnLastPage => Position == 2;
    }
}
