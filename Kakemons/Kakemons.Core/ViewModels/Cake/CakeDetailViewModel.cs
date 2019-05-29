using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Core.Contracts;
using Kakemons.Core.ListView;
using Kakemons.Core.ViewModels.Baker;
using Kakemons.Core.ViewModels.Purchase;
using ReactiveUI;
using Serilog;

namespace Kakemons.Core.ViewModels.Cake
{
    public class CakeDetailViewModel : BaseViewModel
    {
        private readonly IAppUserModelService _appUserModelService;
        private readonly ILogger _logger;
        private readonly Subject<int> _cakeIdSubject;
        private readonly ICakeModelService _cakeModelService;
        private int _cakeId;

        private CakeListItemViewModel _cakeModel;
        private bool _isFavourite;

        public CakeDetailViewModel(
            int bakerId,
            ICakeModelService cakeModelService = null,
            IAppUserModelService appUserModelService = null,
            IScreen hostScreen = null, ILogger logger = null):base(hostScreen)
        {
            _cakeModelService = cakeModelService;
            _appUserModelService = appUserModelService;
            _logger = logger;
            _cakeIdSubject = new Subject<int>();
            ToggleFavoriteCommand = ReactiveCommand.CreateFromTask(ToggleFavorite);
            InitiateChatCommand = ReactiveCommand.CreateFromTask<int>(InitiateChat);
            OrderCakeCommand = ReactiveCommand.CreateFromTask(OrderCake);
            GoToBakerCommand = ReactiveCommand.CreateFromTask<string>(GoToBaker);
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

            Prepare(bakerId);
        }

        private async Task GoToBaker(string bakerId)
        {
            await HostScreen.Router.Navigate.Execute(new BakerProfileViewModel(bakerId, HostScreen, _cakeModelService, _appUserModelService, null, null, _logger));
        }

        public Func<int, Task> ToggleFavoriteFunc { get; set; }

        public CakeListItemViewModel CakeModel
        {
            get => _cakeModel;
            set => this.RaiseAndSetIfChanged(ref _cakeModel, value);
        }

        public ReactiveCommand<string, Unit> GoToBakerCommand { get; set; }


        public ReactiveCommand<int, Unit> InitiateChatCommand { get; }

        public ReactiveCommand<Unit, Unit> OrderCakeCommand { get; }

        public ReactiveCommand<Unit, Unit> ToggleFavoriteCommand { get; set; }

        public void Prepare(int id)
        {
            _cakeId = id;
            var cake = _cakeModelService
                .Cakes
                .Lookup(id);

            if (cake.HasValue)
                MapToViewModel(cake.Value);
        }

        //public override void ViewAppeared() => _cakeIdSubject.OnNext(_cakeId);

        //public override void ViewDisappeared() => CompositeDisposable?.Clear();

        private async Task InitiateChat(int id) => await HostScreen.Router.Navigate.Execute(new ChatViewModel(id, _logger));

        private void MapToViewModel(CakeDto dto) => CakeModel = new CakeListItemViewModel(dto.Id)
        {
            Name = dto.Name,
            Description = dto.Description,
            BakerName = dto.Baker.Fullname,
            BakerId = dto.BakerId,
            Allergens = dto.Allergens,
            TagNames = dto.Tags?.Select(t => t?.Name)?.ToList(),
            ImageSrc = dto.Images?.FirstOrDefault()?.Url,
            IsFavorite = _appUserModelService.FavouriteCakes.Lookup(dto.Id).HasValue
        };

        private async Task OrderCake()
        {
            await HostScreen.Router.Navigate.Execute(new PurchaseViewModel(_cakeId, _cakeModelService));
        }

        private async Task ToggleFavorite()
        {
            var isFavorite = _appUserModelService.FavouriteCakes.Lookup(_cakeId).HasValue;
            if (isFavorite)
                _appUserModelService.FavouriteCakes.Edit(l => l.RemoveKey(_cakeId));
            else
            {
                var cake = _cakeModelService.Cakes.Lookup(_cakeId);
                if (cake.HasValue)
                    _appUserModelService.FavouriteCakes.Edit(l => l.AddOrUpdate(cake.Value));
            }

            CakeModel.IsFavorite = !CakeModel.IsFavorite;
            await _cakeModelService.UpdateFavourite(_appUserModelService.UserId, _cakeId);
        }
    }
}
