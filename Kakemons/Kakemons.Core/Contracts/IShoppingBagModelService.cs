using DynamicData;
using Kakemons.Common.Dtos;

namespace Kakemons.Core.Contracts
{
    public interface IShoppingBagModelService
    {
        IObservableList<CakeDto> ShoppingBag { get; }

        void AddToShoppingBag(CakeDto cake);
        void ClearShoppingBag();
        void RemoveFromShoppingBag(CakeDto cake);
    }
}
