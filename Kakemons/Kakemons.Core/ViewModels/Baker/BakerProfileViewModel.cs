using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using AutoMapper;
using DynamicData;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Core.Contracts;
using Kakemons.Core.ListView;
using Kakemons.Core.Models;
using Kakemons.Core.ViewModels.Cake;
using Kakemons.SDK.ApiContracts;
using ReactiveUI;
using Serilog;

namespace Kakemons.Core.ViewModels.Baker
{
    public class BakerProfileViewModel : BaseViewModel
    {
        private readonly IAppUserModelService _appUserModelService;
        private readonly Subject<string> _bakerIdSubject;
        private readonly IBakerModelService _bakerModelService;
        private readonly ICakeModelService _cakeModelService;
        private readonly IMapper _mapper;
        private readonly IUserApiService _userApiService;
        private readonly ILogger _logger;
        private ReadOnlyObservableCollection<CakeListItemViewModel> _availableCakes;
        private string _bakerId;

        private BakerModel _bakerModel;
        private ReadOnlyObservableCollection<CakeListItemViewModel> _cakesForOrder;

        public BakerProfileViewModel(
            string bakerId,
            IScreen hostScreen = null,
            ICakeModelService cakeModelService = null,
            IAppUserModelService appUserModelService = null,
            IBakerModelService bakerModelService = null,
            IUserApiService userApiService = null,
            ILogger logger = null,
            IMapper mapper = null):base(hostScreen)
        {
            _cakeModelService = cakeModelService;
            _appUserModelService = appUserModelService;
            _bakerModelService = bakerModelService;
            _userApiService = userApiService;
            _logger = logger;
            _mapper = mapper;

            _bakerIdSubject = new Subject<string>();

            StartChatCommand = ReactiveCommand.CreateFromTask(GoToChat);
            PrepareCommand = ReactiveCommand.CreateFromTask(async b => await Prepare(bakerId));

            Observable.Return(Unit.Default)
                .InvokeCommand(PrepareCommand);

            PrepareCommand.Subscribe();
        }

        public ReactiveCommand<Unit, Unit> PrepareCommand { get; set; }

        private async Task GoToChat()
        {
            await HostScreen.Router.Navigate.Execute(new BakerChatViewModel(_bakerId, _appUserModelService, _bakerModelService));
        }

        public ReadOnlyObservableCollection<CakeListItemViewModel> AvailableCakes => _availableCakes;

        public BakerModel BakerModel
        {
            get => _bakerModel;
            set => this.RaiseAndSetIfChanged(ref _bakerModel, value);
        }

        public ReadOnlyObservableCollection<CakeListItemViewModel> CakesForOrder => _cakesForOrder;

        public ReactiveCommand<Unit, Unit> StartChatCommand { get; set; }

        private async Task Prepare(string bakerId)
        {
            _bakerId = bakerId;
            var cakes = _cakeModelService
                .Cakes
                .Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Filter(c => c.BakerId == bakerId)
                .Transform(TransformToListItem)
                .Publish();

            cakes
                .Filter(c => c.Availability == CakeAvailability.Now)
                .Transform(cDto => _mapper.Map<CakeListItemViewModel>(cDto))
                .Bind(out _availableCakes)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(CompositeDisposable);

            cakes
                .Filter(c => c.Availability == CakeAvailability.ForOrder)
                .Transform(cDto => _mapper.Map<CakeListItemViewModel>(cDto))
                .Bind(out _cakesForOrder)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(CompositeDisposable);

            cakes
                .MergeMany(vm => vm.GoToDetailsCommand)
                .Subscribe(async c => await GoToDetails(c))
                .DisposeWith(CompositeDisposable);

            cakes
                .MergeMany(vm => vm.GoToBakerCommand)
                .Subscribe(async c => await GoToBaker(c))
                .DisposeWith(CompositeDisposable);

            cakes
                .MergeMany(vm => vm.ToggleFavoriteCommand)
                .Subscribe(async c => await ToggleFavorite(c))
                .DisposeWith(CompositeDisposable);

            cakes
                .Connect();

            var baker = await _bakerModelService
                .GetBaker(bakerId);

            if (baker != null) MapToViewModel(baker);
        }

        //public override void ViewAppeared() => _bakerIdSubject.OnNext(_bakerId);

        //public override void ViewDisappeared() => CompositeDisposable?.Clear();

        private async Task GoToBaker(string id) => await HostScreen.Router.Navigate.Execute(new BakerProfileViewModel(id, HostScreen, _cakeModelService, _appUserModelService, _bakerModelService, _userApiService, _logger, _mapper));

        private async Task GoToDetails(int id) =>
            await HostScreen.Router.Navigate.Execute(new CakeDetailViewModel(id, _cakeModelService, _appUserModelService,
                HostScreen, _logger));

        private void MapToViewModel(BakerDto dto) => BakerModel = new BakerModel
        {
            Id = dto.Id,
            Name = dto.Fullname,
            Description = dto.Description,
            AvatarUrl = dto.AvatarUrl,
            Address = $"{dto.Address.Street}, {dto.Address.City}",
            Distance = 3d //AppSettings.Position.Distance(baker.Position).ToString("0.# km")
        };

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

        private CakeListItemViewModel TransformToListItem(CakeDto cakeDto) => CakeListItemViewModel.TransformToListItem(
            cakeDto,
            _appUserModelService.FavouriteCakes.Lookup(cakeDto.Id).HasValue);
    }
}
