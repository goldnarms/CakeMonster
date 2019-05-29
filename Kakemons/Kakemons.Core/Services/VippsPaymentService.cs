using System;
using System.Threading.Tasks;
using Kakemons.Common.Enums;

namespace Kakemons.Core.Services
{
    public class VippsPaymentService : PaymentService
    {
        public VippsPaymentService()
        {

        }

        public override Task<PaymentStatus> Pay(double amount, string userId, string bakerId, string description)
        {
            var random = new Random();
            var number = random.Next(10);
            if(number == 3)
            {
                return Task.FromResult(PaymentStatus.Failed);
            }

            return Task.FromResult(PaymentStatus.Success);
        }
    }
}
