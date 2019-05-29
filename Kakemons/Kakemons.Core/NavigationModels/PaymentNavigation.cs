using System.Collections.Generic;
using Kakemons.Common.Dtos;
using Kakemons.Core.ViewModels.Purchase;

namespace Kakemons.Core.NavigationModels
{
    public class PaymentNavigation
    {
        public PaymentNavigation(BakerDto baker, double amount, string bakerId, string description, IEnumerable<OrderLineDto> orderLines)
        {
            Baker = baker;
            Amount = amount;
            BakerId = bakerId;
            Description = description;
            OrderLines = orderLines;
        }

        public BakerDto Baker { get; }
        public double Amount { get; }
        public string BakerId { get; }
        public string Description { get; }
        public IEnumerable<OrderLineDto> OrderLines { get; }
    }
}
