using System;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using DynamicData;
using Kakemons.Common.Models;
using Kakemons.Common.Responses;
using Kakemons.Core.Contracts;
using Kakemons.Core.Extensions;
using Kakemons.SDK.ApiServices;
using Microsoft.AppCenter;
using ReactiveUI;
using Splat;
using Xamarin.Essentials;
using ILogger = Serilog.ILogger;

namespace Kakemons.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ILogger _logger;
        private readonly ISecureBlobCache _secureBlobCache;
        private readonly IBlobCache _localMachineCache;
        private const string ProviderSettingsKey = "providerSettings";
        private const string AccessTokenKey = "accessToken";
        private const string IsNewUserKey = "isNewUser";
        private const string RefreshTokenKey = "refreshToken";
        private readonly Subject<Unit> _settingsObservable = new Subject<Unit>();
        private AccountApiService _accountApiService;
        private string _refreshToken;
        private string _accessToken;

        public SettingsService(ILogger logger = null, ISecureBlobCache secureBlobCache = null, IBlobCache localMachineCache = null)
        {
            _logger = logger ?? Locator.Current.GetService<ILogger>();
            _secureBlobCache = secureBlobCache ?? Locator.Current.GetService<ISecureBlobCache>();
            _localMachineCache = localMachineCache ?? Locator.Current.GetService<IBlobCache>();

            BaseApiUrl = @"http://test.test.no/api";
            var log = _logger.ForContext<SettingsService>();
            log.Information($"Instantiated: {nameof(SettingsService)}");

            // Check and set access token
            GetToken()
                .ToObservable()
                .LogException(log, "Unable to get access token")
                .Subscribe();

            _localMachineCache.GetObject<bool>(IsNewUserKey)
                .Catch<bool, Exception>(ex => Observable.Return(true))
                .Subscribe(newUser => IsNewUser = newUser);
        }

        public string BaseApiUrl { get; }

        public async Task<AuthTokens> GetToken()
        {
            string accessToken = "", refreshToken = "";
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    accessToken = await _secureBlobCache.GetObject<string>(AccessTokenKey);
                    _accessToken = accessToken;
                }
                else
                    accessToken = _accessToken;

                AccessToken = accessToken;
            }
            catch (Exception)
            {
                //var byteArray = Encoding.ASCII.GetBytes($"{Constants.App.ApiKeyUsername}:{Constants.App.ApiKeyPassword}");
                //var token = Convert.ToBase64String(byteArray);
                //return new AuthTokens()
                //{
                //    AccessToken = token,
                //    TokenType = "Basic"
                //};
            }

            try
            {
                if (string.IsNullOrEmpty(_refreshToken))
                {
                    refreshToken = await _secureBlobCache.GetObject<string>(RefreshTokenKey);
                    _refreshToken = refreshToken;
                }
                else
                    refreshToken = _refreshToken;
            }
            catch { };
            return new AuthTokens
            {
                AccessToken = accessToken,
                TokenType = "Bearer",
                RefreshToken = refreshToken
            };
        }

        public async Task SetNewToken()
        {
            try
            {
                var refreshToken = "";
                if (string.IsNullOrEmpty(_refreshToken))
                {
                    refreshToken = await _secureBlobCache.GetObject<string>(RefreshTokenKey);
                    _refreshToken = refreshToken;
                }
                else
                {
                    refreshToken = _refreshToken;
                }

                if (string.IsNullOrEmpty(refreshToken))
                    return;

                AccessToken = string.Empty;
                _accessToken = string.Empty;

                await _secureBlobCache.Invalidate(AccessTokenKey);
                //var newToken = await _accountApiService.RefreshToken(refreshToken, new Context() { { "Url", "RefreshToken" } });
                //await Login(newToken);
            }
            catch (Exception ex)
            {
            };
        }

        public async Task Logout()
        {
            try
            {
                //await _accountApiService.Logout();
            }
            catch (Exception) { }
            AccessToken = string.Empty;
            await _secureBlobCache.Invalidate(AccessTokenKey);
            await _localMachineCache.InsertObject(IsNewUserKey, true);
            IsNewUser = true;
        }

        public async Task Login(LoginUserResponse accessToken)
        {
            _refreshToken = accessToken.RefreshToken;
            _accessToken = accessToken.AccessToken;
            await _secureBlobCache.InsertObject(AccessTokenKey, accessToken.AccessToken);
            await _secureBlobCache.InsertObject(RefreshTokenKey, accessToken.RefreshToken);
            await _localMachineCache.InsertObject(IsNewUserKey, false);
            IsNewUser = false;
            AccessToken = accessToken.AccessToken;
        }

        public string AccessToken { get; set; }

        public bool IsNewUser { get; set; }
    }
}