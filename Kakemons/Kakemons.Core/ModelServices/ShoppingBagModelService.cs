using DynamicData;
using Kakemons.Common.Dtos;
using Kakemons.Core.Contracts;

namespace Kakemons.Core.ModelServices
{
    public class ShoppingBagModelService : IShoppingBagModelService
    {
        private readonly SourceList<CakeDto> _shoppingBag;

        public ShoppingBagModelService()
        {
            _shoppingBag = new SourceList<CakeDto>();
        }

        public void AddToShoppingBag(CakeDto cake)
        {
            _shoppingBag.Edit(l => l.Add(cake));
        }

        public void RemoveFromShoppingBag(CakeDto cake)
        {
            _shoppingBag.Edit(l => l.Remove(cake));
        }

        public void ClearShoppingBag()
        {
            _shoppingBag.Edit(l => l.Clear());
        }

        public IObservableList<CakeDto> ShoppingBag => _shoppingBag;
    }
}
