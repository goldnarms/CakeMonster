using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Common.Requests;
using Kakemons.Core.Contracts;
using Kakemons.Core.NavigationModels;
using Kakemons.SDK.ApiContracts;
using ReactiveUI;
using Splat;

namespace Kakemons.Core.ViewModels.Purchase
{
    public class PaymentViewModel : BaseViewModel
    {
        private readonly IAppUserModelService _appUserModelService;
        private readonly IBakerModelService _bakerModelService;
        private readonly IPaymentApiService _paymentApiService;
        private readonly ObservableAsPropertyHelper<bool> _goNextEnabled;

        public ReactiveCommand<Unit, Unit> PayWithVippsCommand { get; }
        public ReactiveCommand<Unit, Unit> GoNextCommand { get; }

        public PaymentViewModel(
            PaymentNavigation paymentNavigation,
            IAppUserModelService appUserModelService = null,
            IBakerModelService bakerModelService = null,
            IPaymentApiService paymentApiService = null)
        {
            _appUserModelService = appUserModelService ?? Locator.Current.GetService<IAppUserModelService>();
            _bakerModelService = bakerModelService ?? Locator.Current.GetService<IBakerModelService>();
            _paymentApiService = paymentApiService ?? Locator.Current.GetService<IPaymentApiService>();
            PayWithVippsCommand = ReactiveCommand.CreateFromTask(PayWithVipps);
            var canGoToReceiptObservable = this.WhenAnyValue(vm => vm.PaymentSuccessfull)
                .Select(ps => ps)
                .StartWith(false);

            canGoToReceiptObservable.ToProperty(this, vm => vm.GoNextEnabled, out _goNextEnabled);

            GoNextCommand = ReactiveCommand.CreateFromTask(GoToReceipt, canGoToReceiptObservable);

            Prepare(paymentNavigation);
        }

        private Task GoToReceipt()
        {
            throw new NotImplementedException();
        }

        private async Task PayWithVipps()
        {
            var paymentRequest = new PurchaseRequest(_appUserModelService.UserId, BakerId, OrderLines, PaymentProvider.Vipps);
            var result = await _paymentApiService.Pay(paymentRequest);
            if(result == PaymentStatus.Success)
            {
                PaymentMessage = "Betaling gjennomført";
                PaymentSuccessfull = true;
            }
            else
            {
                PaymentMessage = "Kunne ikke gjennomføre betaling, vennligt prøv igjen senere";
                PaymentSuccessfull = false;
            }

        }

        public IEnumerable<OrderLineDto> OrderLines { get; set; }

        public void Prepare(PaymentNavigation payment)
        {
            Amount = payment.Amount;
            BakerId = payment.BakerId;
            Description = payment.Description;
            OrderLines = payment.OrderLines;
        }

        public string Sum { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
        public string BakerName { get; set; }
        public double Amount { get; private set; }
        public string BakerId { get; private set; }
        public string Description { get; private set; }
        public string PaymentMessage { get; private set; }
        public bool GoNextEnabled => _goNextEnabled.Value;

        private bool _paymentSuccessfull;
        public bool PaymentSuccessfull
        {
            get => _paymentSuccessfull;
            set => this.RaiseAndSetIfChanged(ref _paymentSuccessfull, value);
        }
    }
}
