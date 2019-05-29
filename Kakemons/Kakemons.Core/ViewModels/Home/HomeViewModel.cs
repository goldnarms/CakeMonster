using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Core.Contracts;
using Kakemons.Core.ListView;
using Kakemons.Core.ViewModels.Baker;
using Kakemons.Core.ViewModels.Cake;
using Kakemons.Core.ViewModels.Purchase;
using ReactiveUI;
using Splat;
using ILogger = Kakemons.Common.Contracts.ILogger;

namespace Kakemons.Core.ViewModels.Home
{

    public class HomeViewModel : BaseViewModel
    {
        private readonly IScreen _hostScreen;
        private readonly ILogger _logger;
        private readonly ReadOnlyObservableCollection<CakeListItemViewModel> _cakesAvailableNow;
        private readonly ReadOnlyObservableCollection<CakeListItemViewModel> _cakesAvailableForOrder;
        private readonly ICakeModelService _cakeModelService;
        readonly IAppUserModelService _appUserModelService;

        public HomeViewModel(
            IScreen hostScreen = null,
            ICakeModelService cakeModelService = null,
            IAppUserModelService appUserModelService = null,
            ILogger logger = null) : base(hostScreen)
        {
            _hostScreen = hostScreen;
            _logger = logger ?? Locator.Current.GetService<ILogger>();
            _cakeModelService = cakeModelService ?? Locator.Current.GetService<ICakeModelService>();
            _appUserModelService = appUserModelService ?? Locator.Current.GetService<IAppUserModelService>();

            var nearbyCakes = _cakeModelService
                .Cakes
                .Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Transform(TransformToListItem)
                .Publish();

            nearbyCakes
                .Filter(c => c.Availability == CakeAvailability.ForOrder)
                .Bind(out _cakesAvailableForOrder)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(CompositeDisposable);

            nearbyCakes
                .Filter(c => c.Availability == CakeAvailability.Now)
                .Bind(out _cakesAvailableNow)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(CompositeDisposable);

            nearbyCakes
                .MergeMany(vm => vm.GoToDetailsCommand)
                .Subscribe(async c => await GoToDetails(c))
                .DisposeWith(CompositeDisposable);

            nearbyCakes
                .MergeMany(vm => vm.GoToBakerCommand)
                .Subscribe(async c => await GoToBaker(c))
                .DisposeWith(CompositeDisposable);

            nearbyCakes.Connect();


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

        private CakeListItemViewModel TransformToListItem(CakeDto cakeDto)
        {
            var listItem = CakeListItemViewModel.TransformToListItem(cakeDto,
                _appUserModelService.FavouriteCakes.Lookup(cakeDto.Id).HasValue);
            return listItem;
        }

        private async Task GoToDetails(int id)
        {
            await HostScreen.Router.Navigate.Execute(new CakeDetailViewModel(id, _cakeModelService, _appUserModelService, _hostScreen, _logger));
        }

        private async Task GoToBaker(string id)
        {
            await HostScreen.Router.Navigate.Execute(new BakerProfileViewModel(id, _hostScreen, _cakeModelService, _appUserModelService, logger: _logger));
        }

        public ReadOnlyObservableCollection<CakeListItemViewModel> CakesAvailableNow => _cakesAvailableNow;
        public ReadOnlyObservableCollection<CakeListItemViewModel> CakesAvailableForOrder => _cakesAvailableForOrder;
    }
}
