using System;
using System.Threading.Tasks;
using Kakemons.Common.Enums;
using Kakemons.Common.Requests;
using Kakemons.SDK.ApiContracts;

namespace Kakemons.SDK.FakeApiServices
{
    public class FakePaymentApiService : IPaymentApiService
    {
        public Task<PaymentStatus> Pay(PurchaseRequest purchaseRequest)
        {
            var random = new Random();
            var number = random.Next(10);
            if (number == 3)
            {
                return Task.FromResult(PaymentStatus.Failed);
            }

            return Task.FromResult(PaymentStatus.Success);
        }
    }
}
