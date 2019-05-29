using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;
using Kakemons.Core.Contracts;
using Kakemons.SDK.ApiServiceContracts;

namespace Kakemons.Core.ModelServices
{
    public class BakerModelService : IBakerModelService
    {
        private readonly IBakerApiService _bakerApiService;
        public IObservableCache<BakerDto, string> Bakers { get; }

        public BakerModelService(IBakerApiService bakerApiService)
        {
            _bakerApiService = bakerApiService;
        }

        public async Task<BakerDto> GetBaker(string id)
        {
            return await _bakerApiService.GetBaker(id);
        }
    }
}
