using System;
using Kakemons.Common.Enums;
using Kakemons.Core.Contracts;
using Kakemons.Core.Services;

namespace Kakemons.Core.Factories
{
    public class PaymentFactory : IPaymentFactory
    {
        public PaymentFactory()
        {

        }

        public IPaymentService GetPaymentServiceForProvider(PaymentProvider paymentProvider)
        {
            switch (paymentProvider)
            {
                case PaymentProvider.Vipps:
                    return new VippsPaymentService();
                case PaymentProvider.Stripe:
                    return new VippsPaymentService(); //TODO: Replace
                default:
                    throw new Exception();
            }
        }
    }
}
