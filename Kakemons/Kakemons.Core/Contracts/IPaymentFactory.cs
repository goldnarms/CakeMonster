using Kakemons.Common.Enums;

namespace Kakemons.Core.Contracts
{
    public interface IPaymentFactory
    {
        IPaymentService GetPaymentServiceForProvider(PaymentProvider paymentProvider);
    }
}
