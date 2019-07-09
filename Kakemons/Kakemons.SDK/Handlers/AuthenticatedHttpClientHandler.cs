using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Kakemons.Common.Models;
using Serilog;
using Serilog.Events;

namespace Kakemons.SDK.Handlers
{
    public class AuthenticatedHttpClientHandler: HttpClientHandler
    {
        private readonly Func<Task<AuthTokens>> _getToken;
        private readonly Func<Task> _setNewToken;
        private readonly ILogger _logger;
        private readonly string _appVersion;
        private readonly string _platform;
        private readonly string _osVersion;
        private readonly string _locale;

        /// <summary>
        /// If user is authenticated, use Bearer token, if not use basic with api credentials
        /// </summary>
        /// <param name="getToken"></param>
        /// <param name="logger"></param>
        /// <param name="appVersion"></param>
        /// <param name="platform"></param>
        /// <param name="osVersion"></param>
        /// <param name="locale"></param>
        public AuthenticatedHttpClientHandler(Func<Task<AuthTokens>> getToken, Func<Task> setNewToken, ILogger logger, string appVersion = "", string platform = "", string osVersion = "", string locale = "")
        {
            if (getToken == null) throw new ArgumentNullException(nameof(getToken));
            _getToken = getToken;
            _setNewToken = setNewToken;
            _logger = logger;
            _appVersion = appVersion;
            _platform = platform;
            _osVersion = osVersion;
            _locale = locale;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _getToken().ConfigureAwait(false);
            HttpResponseMessage response;
            _logger.Write(LogEventLevel.Information, "Request:");
            _logger.Write(LogEventLevel.Information, request.ToString());
            if (request.Content != null)
            {
                _logger.Write(LogEventLevel.Information, await request.Content.ReadAsStringAsync());
            }

            _logger.Write(LogEventLevel.Information, "\n");
            AppendHeaders(request);
            if (accessToken == null || string.IsNullOrEmpty(accessToken.AccessToken))
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Request failed");
                    throw;
                }
            }
            else
            {
                try
                {
                    response = await SendRequestAsync(request, cancellationToken, accessToken.TokenType, accessToken.AccessToken);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await _setNewToken();
                        var newAccessToken = await _getToken().ConfigureAwait(false);
                        response = await SendRequestAsync(request, cancellationToken, newAccessToken?.TokenType, newAccessToken?.AccessToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Request failed");
                    throw;
                }
            }
            
            _logger.Write(LogEventLevel.Information, $"Response for {response.RequestMessage?.RequestUri?.AbsoluteUri}:");
            _logger.Write(LogEventLevel.Information, response.ToString());
            if (response.Content != null)
            {
                _logger.Write(LogEventLevel.Information, await response.Content.ReadAsStringAsync());
            }

            _logger.Write(LogEventLevel.Information, "\n");

            return response;
        }

        private void AppendHeaders(HttpRequestMessage request)
        {
            var localeHeader = "X-Locale";
            var appVersionHeader = "X-AppVersion";
            var platformHeader = "X-Platform";
            var osVersionHeader = "X-OSVersion";
            if (request.Headers.Contains(localeHeader))
                request.Headers.Remove(localeHeader);
            if (request.Headers.Contains(appVersionHeader))
                request.Headers.Remove(appVersionHeader);
            if (request.Headers.Contains(platformHeader))
                request.Headers.Remove(platformHeader);
            if (request.Headers.Contains(osVersionHeader))
                request.Headers.Remove(osVersionHeader);
            request.Headers.Add(localeHeader, _locale);
            request.Headers.Add(appVersionHeader, _appVersion);
            request.Headers.Add(platformHeader, _platform);
            request.Headers.Add(osVersionHeader, _osVersion);
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken, string scheme, string token)
        {
            if(!string.IsNullOrEmpty(scheme) && !string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue(scheme, token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
