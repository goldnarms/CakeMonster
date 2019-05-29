using Kakemons.Common.Enums;

namespace Kakemons.Core.ViewModels.Purchase
{
    public abstract class PaymentResult
    {
        public PaymentStatus Status { get; set; }
        public string Message { get; set; }
        public string ExternalId { get; set; }
    }
}
