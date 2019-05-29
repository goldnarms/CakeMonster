using System;
using Kakemons.Core.NavigationModels;
using ReactiveUI;
using Serilog;

namespace Kakemons.Core.ViewModels.Cake
{
    public class ReceiptViewModel : BaseViewModel
    {
        private string _bakerName;
        private string _cakeName;
        private string _imageUrl;
        private string _pickupAddres;
        private DateTimeOffset _pickupDate;
        private DateTimeOffset _purchaseDate;

        public ReceiptViewModel(ReceiptNavigation receiptNavigation, ILogger log)
        {
            Prepare(receiptNavigation);
        }

        public DateTimeOffset PurchaseDate
        {
            get => _purchaseDate;
            set => this.RaiseAndSetIfChanged(ref _purchaseDate, value);
        }

        public string CakeName
        {
            get => _cakeName;
            set => this.RaiseAndSetIfChanged(ref _cakeName, value);
        }

        public DateTimeOffset PickupDate
        {
            get => _pickupDate;
            set => this.RaiseAndSetIfChanged(ref _pickupDate, value);
        }

        public string PickupAddres
        {
            get => _pickupAddres;
            set => this.RaiseAndSetIfChanged(ref _pickupAddres, value);
        }

        public string BakerName
        {
            get => _bakerName;
            set => this.RaiseAndSetIfChanged(ref _bakerName, value);
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set => this.RaiseAndSetIfChanged(ref _imageUrl, value);
        }

        public void Prepare(ReceiptNavigation parameter) => MapToViewModel(parameter);

        private void MapToViewModel(ReceiptNavigation receiptNavigation)
        {
            PurchaseDate = receiptNavigation.PurchaseDate;
            CakeName = receiptNavigation.CakeName;
            ImageUrl = receiptNavigation.ImageUrl;
            BakerName = receiptNavigation.BakerName;
            PickupAddres = receiptNavigation.PickupAddress;
            PickupDate = receiptNavigation.PickupDate;
        }
    }
}
