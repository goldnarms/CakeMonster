using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Core.Contracts;
using Kakemons.Core.ListView;
using Kakemons.Core.ModelServices;
using Kakemons.Core.ViewModels.Baker;
using Kakemons.Core.ViewModels.Cake;
using ReactiveUI;
using Splat;
using ILogger = Kakemons.Common.Contracts.ILogger;

namespace Kakemons.Core.ViewModels.Favorites
{
    public class FavoritesViewModel : BaseViewModel
    {
        private readonly IScreen _hostScreen;
        private readonly ILogger _logger;
        private readonly ICakeModelService _cakeModelService;
        private readonly IAppUserModelService _appUserModelService;
        private readonly ReadOnlyObservableCollection<CakeListItemViewModel> _favoriteCakes;
        private readonly CompositeDisposable _cd;

        public FavoritesViewModel(IScreen hostScreen = null, IAppUserModelService appUserModelService = null, ICakeModelService cakeModelService = null, ILogger logger = null):base(hostScreen)
        {
            _hostScreen = hostScreen;
            _logger = logger ?? Locator.Current.GetService<ILogger>();
            _cakeModelService = cakeModelService ?? Locator.Current.GetService<ICakeModelService>();
            _appUserModelService = appUserModelService ?? Locator.Current.GetService<IAppUserModelService>();

            _cd = new CompositeDisposable();

            var favoriteCakes = _appUserModelService
                .FavouriteCakes
                .Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Transform(TransformToListItem)
                .Publish();

            favoriteCakes
                .Bind(out _favoriteCakes)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(_cd);

            favoriteCakes
                .MergeMany(vm => vm.GoToDetailsCommand)
                .Subscribe(async c => await GoToDetails(c))
                .DisposeWith(_cd);

            favoriteCakes
                .MergeMany(vm => vm.GoToBakerCommand)
                .Subscribe(async c => await GoToBaker(c))
                .DisposeWith(_cd);

            favoriteCakes
                .MergeMany(vm => vm.ToggleFavoriteCommand)
                .Subscribe(async c => await ToggleFavorite(c))
                .DisposeWith(_cd);

            favoriteCakes.Connect();

            ToggleFavoriteFunc = new Func<int, Task>(async id =>
            {
                var isFavorite = _appUserModelService.FavouriteCakes.Lookup(id).HasValue;
                if (isFavorite)
                    _appUserModelService.FavouriteCakes.Edit(l => l.RemoveKey(id));
                else
                {
                    var cake = _cakeModelService.Cakes.Lookup(id);
                    if (cake.HasValue)
                        _appUserModelService.FavouriteCakes.Edit(l => l.AddOrUpdate(cake.Value));
                }

                await _cakeModelService.UpdateFavourite(_appUserModelService.UserId, id);
            });
        }

        public Func<int, Task> ToggleFavoriteFunc { get; set; }

        private async Task ToggleFavorite(int id)
        {

            var isFavorite = _appUserModelService.FavouriteCakes.Lookup(id).HasValue;
            if (isFavorite)
            {
                _appUserModelService.RemoveFromFavorites(id);
            }
            else
            {
                var cake = _cakeModelService.Cakes.Lookup(id);
                if (cake.HasValue)
                    _appUserModelService.AddToFavorites(id, cake.Value);
            }

            await _cakeModelService.UpdateFavourite(_appUserModelService.UserId, id);
        }

        private async Task GoToDetails(int id)
        {
            await HostScreen.Router.Navigate.Execute(new CakeDetailViewModel(id, _cakeModelService, _appUserModelService, _hostScreen, _logger));
        }

        private async Task GoToBaker(string id)
        {
            await HostScreen.Router.Navigate.Execute(new BakerProfileViewModel(id, _hostScreen, _cakeModelService, _appUserModelService, logger: _logger));
        }

        public ReadOnlyObservableCollection<CakeListItemViewModel> FavoriteCakes => _favoriteCakes;

        private CakeListItemViewModel TransformToListItem(CakeDto cakeDto)
        {
            return CakeListItemViewModel.TransformToListItem(cakeDto,
                _appUserModelService.FavouriteCakes.Lookup(cakeDto.Id).HasValue);
        }
    }
}
