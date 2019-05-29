using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using DynamicData;
using Kakemons.Common.Contracts;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Core.Contracts;
using Kakemons.Core.ListView;
using Kakemons.Core.ViewModels.Baker;
using Kakemons.Core.ViewModels.Cake;
using MvvmCross.Navigation;
using ReactiveUI;

namespace Kakemons.Core.ViewModels.Search
{
    public class SearchViewModel: BaseViewModel
    {
        private readonly ICakeModelService _cakeModelService;
        private readonly IAppUserModelService _appUserModelService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxNavigationService _navigationService;
        private readonly CompositeDisposable _cd;
        private readonly ReadOnlyObservableCollection<CakeListItemViewModel> _popularSearches;
        private readonly ReadOnlyObservableCollection<CakeListItemViewModel> _searchResults;

        public SearchViewModel(
            ICakeModelService cakeModelService,
            IAppUserModelService appUserModelService,
            IUserDialogs userDialogs,
            IMvxNavigationService navigationService,
            ILogger logger)
        {
            _cakeModelService = cakeModelService;
            _appUserModelService = appUserModelService;
            _userDialogs = userDialogs;
            _navigationService = navigationService;
            _cd = new CompositeDisposable();

            var searchFilter = this.ObservableForProperty(vm => vm.Query)
                .Value()
                .StartWith("")
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromMilliseconds(400))
                .Select(SearchPredicate);

            var cakeTypeFilter = this.ObservableForProperty(vm => vm.CakeTypeFilter)
                .Value()
                .StartWith(CakeType.None)
                .DistinctUntilChanged()
                .Select(CakeTypePredicate);

            var nearbyCakes = cakeModelService
                .Cakes
                .Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Transform(TransformToListItem)
                .Publish();

            nearbyCakes
                .Bind(out _popularSearches)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(_cd);

            nearbyCakes
                .Filter(searchFilter)
                .Filter(cakeTypeFilter)
                .Bind(out _searchResults)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(_cd);

            nearbyCakes
                .MergeMany(vm => vm.GoToDetailsCommand)
                .Subscribe(async c => await GoToDetails(c))
                .DisposeWith(_cd);

            nearbyCakes
                .MergeMany(vm => vm.GoToBakerCommand)
                .Subscribe(async c => await GoToBaker(c))
                .DisposeWith(_cd);

            nearbyCakes
                .MergeMany(vm => vm.ToggleFavoriteCommand)
                .Subscribe(async c => await ToggleFavorite(c))
                .DisposeWith(_cd);

            nearbyCakes.Connect();

            FilterByTypeCommand = ReactiveCommand.Create(FilterByType);

            var cakeTypeOptions = Enum.GetNames(typeof(CakeType)).Select(cakeType => new ActionSheetOption(cakeType, () => SetCakeType((CakeType)Enum.Parse(typeof(CakeType), cakeType)))).ToList();
            _sheetConfig = new ActionSheetConfig
            {
                Title = "Kaketype",
                UseBottomSheet = true,
                Options = cakeTypeOptions
            };
        }

        private void SetCakeType(CakeType cakeType)
        {
            CakeTypeFilter = cakeType;
        }

        private CakeType _cakeTypeFilter = CakeType.None;

        public CakeType CakeTypeFilter
        {
            get => _cakeTypeFilter;
            set => this.RaiseAndSetIfChanged(ref _cakeTypeFilter, value);
        }

        public ReactiveCommand<Unit, Unit> FilterByTypeCommand { get; set; }

        private void FilterByType()
        {
            _userDialogs.ActionSheet(_sheetConfig);
        }

        public ReadOnlyObservableCollection<CakeListItemViewModel> PopularSearches => _popularSearches;

        public ReadOnlyObservableCollection<CakeListItemViewModel> SearchResults => _searchResults;

        private async Task ToggleFavorite(int id)
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
        }

        private async Task GoToDetails(int id)
        {
            await _navigationService.Navigate<CakeDetailViewModel, int>(id);
        }

        private async Task GoToBaker(string id)
        {
            await _navigationService.Navigate<BakerProfileViewModel, string>(id);
        }

        private CakeListItemViewModel TransformToListItem(CakeDto cakeDto)
        {
            return CakeListItemViewModel.TransformToListItem(cakeDto,
                _appUserModelService.FavouriteCakes.Lookup(cakeDto.Id).HasValue);
        }

        private Func<CakeListItemViewModel, bool> SearchPredicate(string query)
        {
            return s => (!string.IsNullOrEmpty(query) && query.Length > 3) && (s.Name.ToLower().Contains(query.ToLower()) || s.TagNames.Any(sp => sp.ToLower().Contains(query.ToLower())));
        }

        private Func<CakeListItemViewModel, bool> CakeTypePredicate(CakeType cakeType)
        {
            return ct => cakeType == CakeType.None || ct.CakeType == cakeType;
        }

        private string _query;
        private ActionSheetConfig _sheetConfig;

        public string Query
        {
            get => _query;
            set => this.RaiseAndSetIfChanged(ref _query, value);
        }
    }
}
