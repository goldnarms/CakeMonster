using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Enums;
using Kakemons.Common.Requests;
using Kakemons.Core.ViewModels.Purchase;
using Kakemons.SDK.ApiContracts;

namespace Kakemons.SDK.ApiServices
{
    public class PaymentApiService : IPaymentApiService
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
