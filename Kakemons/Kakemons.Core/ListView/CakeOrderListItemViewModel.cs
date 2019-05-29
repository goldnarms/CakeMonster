using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using ReactiveUI;

namespace Kakemons.Core.ListView
{
    public class CakeOrderListItemViewModel : ReactiveObject
    {
        private int _id;

        public CakeOrderListItemViewModel(int id)
        {
            Id = id;

            //RemoveCommand = ReactiveCommand.Create<Unit, int>(_ => Id);
        }

        public int Id
        {
            get => _id;
            private set => _id = value;
        }

        //public ReactiveCommand<Unit, int> RemoveCommand { get; }

        public CakeDto CakeModel { get; set; }
        public static CakeOrderListItemViewModel TransformToListItem(CakeDto cakeDto)
        {
            return new CakeOrderListItemViewModel(cakeDto.Id)
            {
                CakeModel = cakeDto,
            };
        }
    }
}
