using System;
using System.Collections.Generic;
using System.Text;

namespace Kakemons.Core.NavigationModels
{
    public class ReceiptNavigation
    {
        public string CakeName { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public string ImageUrl { get; set; }
        public string BakerName { get; set; }
        public string PickupAddress { get; set; }
        public DateTimeOffset PickupDate { get; set; }
    }
}
