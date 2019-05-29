using System.Threading.Tasks;
using Kakemons.Common.Enums;
using Kakemons.Core.Contracts;

namespace Kakemons.Core.Services
{
    public abstract class PaymentService : IPaymentService
    {
        public abstract Task<PaymentStatus> Pay(double amount, string userId, string bakerId, string description);
    }
}
