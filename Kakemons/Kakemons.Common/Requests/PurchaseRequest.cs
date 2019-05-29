using System.Collections.Generic;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Core.ViewModels.Purchase;

namespace Kakemons.Common.Requests
{
    public class PurchaseRequest
    {
        public string UserId { get; }
        public string BakerId { get; }
        public IEnumerable<OrderLineDto> OrderItems { get; }
        public PaymentProvider PaymentProvider { get; }

        public PurchaseRequest(string userId, string bakerId, IEnumerable<OrderLineDto> orderItems, PaymentProvider paymentProvider)
        {
            UserId = userId;
            BakerId = bakerId;
            OrderItems = orderItems;
            PaymentProvider = paymentProvider;
        }
    }
}
