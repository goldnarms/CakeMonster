using System.Net.Http;
using Refit;
using Serilog;

namespace Kakemons.SDK.ApiServices
{
    public class BaseApiService<T>
    {
        protected readonly T Api;
        protected ILogger Logger { get; }
        public BaseApiService(ILogger logger, HttpClient client)
        {
            Api = RestService.For<T>(client);
            Logger = logger;
        }
    }
}
