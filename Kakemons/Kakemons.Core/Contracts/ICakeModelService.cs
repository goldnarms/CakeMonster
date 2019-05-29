using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;

namespace Kakemons.Core.Contracts
{
    public interface ICakeModelService
    {
        Task UpdateFavourite(string userId, int id);
        IObservableCache<CakeDto, int> Cakes { get; }
    }
}
