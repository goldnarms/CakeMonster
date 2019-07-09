using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Kakemons.SDK.Handlers
{
    public class AuthenticatedHttpClientDelegatingHandler : DelegatingHandler
    {
        private readonly Func<Task<Tuple<string, string>>> _getToken;

        /// <summary>
        /// If user is authenticated, use Bearer token, if not use basic with api credentials
        /// </summary>
        /// <param name="messageHandler"></param>
        /// <param name="getToken"></param>
        public AuthenticatedHttpClientDelegatingHandler(HttpMessageHandler messageHandler, Func<Task<Tuple<string, string>>> getToken):base(messageHandler)
        {
            if (getToken == null) throw new ArgumentNullException(nameof(getToken));
            _getToken = getToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _getToken().ConfigureAwait(false);
            if(accessToken == null)
                return await base.SendAsync(request, cancellationToken);
            return await SendRequestAsync(request, cancellationToken, accessToken.Item1, accessToken.Item2);
            /*
            if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized) 
            {
                var token = await _getToken(true).ConfigureAwait(false);
                return await SendRequestAsync(request, cancellationToken, token.Item1, token.Item2);
            }
            */
            //return response;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken, string scheme, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
