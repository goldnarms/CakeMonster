using System.Threading.Tasks;
using Kakemons.Common.Responses;

namespace Kakemons.SDK.ApiServiceContracts
{
    public interface IAccountApiService
    {
        Task<LoginUserResponse> LoginUser(string username, string password);
    }
}
