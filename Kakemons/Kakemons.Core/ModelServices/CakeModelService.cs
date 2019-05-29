using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;
using Kakemons.Core.Contracts;
using Kakemons.Core.Extensions;
using Kakemons.SDK.ApiContracts;
using ReactiveUI;
using Serilog;

namespace Kakemons.Core.ModelServices
{
    public class CakeModelService : ICakeModelService
    {
        private readonly ICakeApiService _cakeApiService;
        private readonly IPaymentApiService _paymentApiService;
        private readonly SourceCache<CakeDto, int> _nearbyCakes;
        private readonly CompositeDisposable _cd;
        private string _cacheKey = "nearby_cakes";

        public CakeModelService(
            ICakeApiService cakeApiService,
            IAppUserModelService appUserModelService,
            IPaymentApiService paymentApiService,
            ILogger logger)
        {
            _cakeApiService = cakeApiService;
            _paymentApiService = paymentApiService;
            _nearbyCakes = new SourceCache<CakeDto, int>(c => c.Id);
            _cd = new CompositeDisposable();

            //_nearbyCakes
            //    .Connect()
            //    .CacheChangeSet(_cacheKey, logger)
            //    .Subscribe();

            appUserModelService
                .UserObservable
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Where(u => u != null)
                .SelectMany(async u => await cakeApiService.GetNearbyCakes(u.Position))
                .RetryWithBackoff(3)
                .LogException(logger)
                .Subscribe(cakes =>
                    {
                        _nearbyCakes.Edit(l => l.AddOrUpdate(cakes));
                    });

            //Observable.FromAsync(async () => await _nearbyCakes.FromCache(_cacheKey, EqualityComparer<CakeDto>.Default)).Subscribe().DisposeWith(_cd);
        }

        public IObservableCache<CakeDto, int> NearbyCakes => _nearbyCakes;

        public Task UpdateFavourite(string userId, int id)
        {
            return Task.CompletedTask;
        }

        public IObservableCache<CakeDto, int> Cakes => _nearbyCakes;
    }
}
