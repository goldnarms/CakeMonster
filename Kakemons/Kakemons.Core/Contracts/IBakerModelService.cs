using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;

namespace Kakemons.Core.Contracts
{
    public interface IBakerModelService
    {
        IObservableCache<BakerDto, string> Bakers { get; }
        Task<BakerDto> GetBaker(string id);
    }
}
