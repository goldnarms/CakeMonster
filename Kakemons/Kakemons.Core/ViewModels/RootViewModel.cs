using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.Favorites;
using Kakemons.Core.ViewModels.Home;
using Kakemons.Core.ViewModels.Search;
using Serilog;

namespace Kakemons.Core.ViewModels
{
    public class RootViewModel: BaseViewModel
    {
        private readonly ILogger _logger;
        private int _itemIndex;

        public RootViewModel(ILogger logger)
        {
            _logger = logger;
        }

        public int ItemIndex
        {
            get => _itemIndex;
            set
            {
                if (_itemIndex == value)
                    return;
                _itemIndex = value;
                //RaisePropertyChanged(() => ItemIndex);
            }
        }

        public async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            try
            {
                //tasks.Add(_navigationService.Navigate<HomeViewModel>());
                //tasks.Add(_navigationService.Navigate<SearchViewModel>());
                //tasks.Add(_navigationService.Navigate<FavoritesViewModel>());
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.Error(GetType().Name, ex);
            }
        }
    }
}
