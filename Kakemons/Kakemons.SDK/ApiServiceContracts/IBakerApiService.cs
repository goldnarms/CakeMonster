using System.Threading.Tasks;
using Kakemons.Common.Dtos;

namespace Kakemons.SDK.ApiServiceContracts
{
    public interface IBakerApiService
    {
        Task<BakerDto> GetBaker(string id);
    }
}
