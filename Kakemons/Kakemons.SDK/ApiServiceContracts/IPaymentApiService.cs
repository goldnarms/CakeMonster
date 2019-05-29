using System.Collections.Generic;
using System.Threading.Tasks;
using Kakemons.Common.Enums;
using Kakemons.Common.Requests;
using Kakemons.Core.ViewModels.Purchase;

namespace Kakemons.SDK.ApiContracts
{
    public interface IPaymentApiService
    {
        Task<PaymentStatus> Pay(PurchaseRequest purchaseRequest);
    }
}
