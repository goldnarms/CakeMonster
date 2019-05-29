using System;
using System.Net.Http;
using System.Threading.Tasks;
using Kakemons.Common.Requests;
using Kakemons.Common.Responses;
using Kakemons.SDK.ApiContracts;
using Kakemons.SDK.ApiServiceContracts;
using Serilog;

namespace Kakemons.SDK.ApiServices
{
    public class AccountApiService : BaseApiService<IAccountApi>, IAccountApiService
    {
        public AccountApiService(ILogger logger, HttpClient client) : base(logger, client)
        {
        }

        public async Task<LoginUserResponse> LoginUser(string username, string password)
        {
            try
            {
                return await Api.Login(new LoginRequest(username, password));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, nameof(LoginUser));
                return null;
            }
        }
    }
}
