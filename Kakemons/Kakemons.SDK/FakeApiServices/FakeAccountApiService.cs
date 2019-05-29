using System.Threading.Tasks;
using Kakemons.Common.Responses;
using Kakemons.SDK.ApiContracts;
using Kakemons.SDK.ApiServiceContracts;

namespace Kakemons.SDK.FakeApiServices
{
    public class FakeAccountApiService : IAccountApiService
    {
        public FakeAccountApiService()
        {
            
        }

        public Task<LoginUserResponse> LoginUser(string username, string password)
        {
            return Task.FromResult(new LoginUserResponse(true, "123", "1"));
        }
    }
}
