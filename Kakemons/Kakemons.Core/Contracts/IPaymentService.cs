using System.Threading.Tasks;
using Kakemons.Common.Enums;

namespace Kakemons.Core.Contracts
{
    public interface IPaymentService
    {
        Task<PaymentStatus> Pay(double amount, string userId, string bakerId, string description);
    }
}
