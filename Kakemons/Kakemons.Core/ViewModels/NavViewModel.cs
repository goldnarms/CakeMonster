using System.Threading.Tasks;
using Kakemons.Common.Contracts;

namespace Kakemons.Core.ViewModels
{
    public class NavViewModel: BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public NavViewModel(IMvxNavigationService navigationService, ILogger logger)
        {
            _navigationService = navigationService;
        }

        public async Task NavigateToRoot()
        {
            await _navigationService.Navigate<RootViewModel>();
        }
    }
}
