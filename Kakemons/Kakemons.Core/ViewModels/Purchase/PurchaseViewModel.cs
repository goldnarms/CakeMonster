using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Kernel;
using Kakemons.Common.Dtos;
using Kakemons.Core.Contracts;
using Kakemons.Core.ListView;
using ReactiveUI;
using Splat;

namespace Kakemons.Core.ViewModels.Purchase
{
    public class PurchaseViewModel : BaseViewModel
    {
        private readonly ICakeModelService _cakeModelService;
        private readonly IShoppingBagModelService _shoppingBagModelService;
        private readonly ReadOnlyObservableCollection<CakeOrderListItemViewModel> _shoppingBag;
        private readonly Subject<int> _cakeIdSubject;
        private int _cakeId;

        public PurchaseViewModel(int cakeId, ICakeModelService cakeModelService = null, IShoppingBagModelService shoppingBagModelService = null)
        {
            _cakeModelService = cakeModelService ?? Locator.Current.GetService<ICakeModelService>();
            _shoppingBagModelService = shoppingBagModelService ?? Locator.Current.GetService<IShoppingBagModelService>();

            var shoppingBag = _shoppingBagModelService
                .ShoppingBag
                .Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Transform(TransformToListItem)
                .Publish();

            shoppingBag
                .Bind(out _shoppingBag)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(CompositeDisposable);

            shoppingBag.Connect();

            RemoveItemFunc = new Func<int, Task>(async cId =>
            {
                var cakeOptional = _cakeModelService.Cakes.Lookup(cId);
                if (cakeOptional.HasValue)
                    _shoppingBagModelService.RemoveFromShoppingBag(cakeOptional.Value);
            });

            Prepare(cakeId);
        }

        public Func<int, Task> RemoveItemFunc { get; set; }

        public void Prepare(int cakeId)
        {
            var cakeOptional = _cakeModelService.Cakes.Lookup(cakeId);
            if (cakeOptional.HasValue)
                _shoppingBagModelService.AddToShoppingBag(cakeOptional.Value);
        }

        public ReadOnlyObservableCollection<CakeOrderListItemViewModel> ShoppingBag => _shoppingBag;

        private CakeOrderListItemViewModel TransformToListItem(CakeDto cakeDto)
        {
            var listItem = CakeOrderListItemViewModel.TransformToListItem(cakeDto);
            return listItem;
        }

        //public override void ViewAppeared()
        //{
        //    _cakeIdSubject.OnNext(_cakeId);
        //}

        //public override void ViewDisappeared()
        //{
        //    CompositeDisposable?.Clear();
        //}


    }
}
