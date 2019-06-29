using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Energihjem.Mobile.Core.Contracts;
using Kakemons.Common.Models;
using Splat;
using ILogger = Serilog.ILogger;

namespace Kakemons.Core.Services
{
    public static class HttpClientService
    {
        private static HttpClient _httpClient;
        public static HttpClient Instance => _httpClient ?? (_httpClient = SetupFreshHttpClient());

        public static HttpClient SetupHttpClient(Func<Task<AuthTokens>> getToken, Func<Task> setToken, string apiUrl, ILogger logger)
        {
            var httpClient = new HttpClient(new AuthenticatedHttpClientHandler(getToken, setToken,logger,
                Xamarin.Essentials.VersionTracking.CurrentVersion,
                Xamarin.Essentials.DeviceInfo.Platform.ToString(),
                Xamarin.Essentials.DeviceInfo.VersionString,
                CultureInfo.CurrentCulture.CompareInfo.Name))
            {
                BaseAddress = new Uri(apiUrl),
                Timeout = TimeSpan.FromMinutes(5)
            };
            httpClient.DefaultRequestHeaders.ConnectionClose = false;
            

            return httpClient;
        }

        private static HttpClient SetupFreshHttpClient()
        {
            var settingsService = Locator.Current.GetService<ISettingsService>();
            var logger = Locator.Current.GetService<ILogger>();
            return SetupHttpClient(settingsService.GetToken, settingsService.SetNewToken, settingsService.BaseApiUrl, logger);
        }
    }
}
