using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Akavache;
using DynamicData;
using Kakemons.Common.Dtos;
using Kakemons.Core.Contracts;
using Kakemons.Core.Extensions;
using Kakemons.Core.Helpers;
using Kakemons.SDK.ApiContracts;
using ReactiveUI;
using Serilog;
using Xamarin.Essentials;


namespace Kakemons.Core.ModelServices
{
    public class AppUserModelService : IAppUserModelService
    {
        private readonly SourceCache<CakeDto, int> _favouriteCakes;
        readonly IAppSettings _appSettings;
        private readonly BehaviorSubject<string> _userIdSubject;
        private readonly ISecureBlobCache _blobCache;
        private readonly BehaviorSubject<UserDto> _userSubject;
        private readonly CompositeDisposable _cd;
        private const string CacheKey = "favorite_cakes";
        private const string AccessTokenKey = "access_token";
        private const string IsNewUserKey = "is_new_user";

        public AppUserModelService(IUserApiService userApiService, ILogger logger, IAppSettings appSettings)
        {
            _favouriteCakes = new SourceCache<CakeDto, int>(c => c.Id);
            _appSettings = appSettings;
            _userIdSubject = new BehaviorSubject<string>(String.Empty);
            _userSubject = new BehaviorSubject<UserDto>(null);
            _cd = new CompositeDisposable();

            _favouriteCakes
                .Connect()
                .CacheChangeSet(CacheKey, logger)
                .Subscribe();

            _userIdSubject
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Where(u => u != null)
                .SelectMany(async uId => await userApiService.GetFavourites(uId))
                .RetryWithBackoff(3)
                .LogException(logger)
                .Subscribe(cakes =>
                {
                    _favouriteCakes.Edit(l => l.AddOrUpdate(cakes));
                })
                .DisposeWith(_cd);

            Observable.FromAsync(async () => await _favouriteCakes.FromCache(CacheKey, EqualityComparer<CakeDto>.Default)).Subscribe().DisposeWith(_cd);
        }

        public string UserId => _userSubject?.Value?.Id ?? String.Empty;
        public UserDto User => _userSubject?.Value ?? null;

        public async Task<UserDto> GetUser()
        {
            var user = await _appSettings.GetUser();
            if (user != null)
            {
                return user;
            }

            if (_userSubject.Value != null)
            {
                await _appSettings.SetUser(_userSubject.Value);
                return _userSubject.Value;
            }

            return null;
        }

        public bool IsLoggedIn => User != null;

        public void LogInUser(UserDto user)
        {
            _userSubject.OnNext(user);
            IsNewUser = false;
        }

        public async Task LogInUser(UserDto user, string accessToken)
        {
            _userSubject.OnNext(user);
            await SecureStorage.SetAsync(AccessTokenKey, accessToken);
            await BlobCache.LocalMachine.InsertObject(IsNewUserKey, false);
            IsNewUser = false;
            AccessToken = accessToken;
        }

        public async Task LogOutUser()
        {
            _userSubject.OnNext(null);
            SecureStorage.Remove(AccessTokenKey);
            await BlobCache.LocalMachine.InsertObject(IsNewUserKey, true);
            IsNewUser = true;
        }

        public void AddToFavorites(int cakeId, CakeDto cake)
        {
            var hasCake = _favouriteCakes.Lookup(cakeId);
            if (!hasCake.HasValue)
            {
                _favouriteCakes.AddOrUpdate(cake);
            }
        }

        public void RemoveFromFavorites(int cakeId)
        {
            _favouriteCakes.Edit(l => l.RemoveKey(cakeId));
        }

        public IObservable<UserDto> UserObservable => _userSubject;

        public SourceCache<CakeDto, int> FavouriteCakes => _favouriteCakes;

        public async Task<Tuple<string, string>> GetToken()
        {
            string accessToken;
            try
            {
                accessToken = await SecureStorage.GetAsync(AccessTokenKey);
                AccessToken = accessToken;
            }
            catch (Exception)
            {
                return null; // No token was found. User has yet to be authenticated
            }

            return new Tuple<string, string>("Bearer", accessToken);
        }

        public string AccessToken { get; private set; }
        public bool IsNewUser { get; private set; }
    }
}
