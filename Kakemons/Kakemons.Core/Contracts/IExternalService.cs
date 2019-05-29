using System.Threading.Tasks;
using Kakemons.Common.Responses;

namespace Kakemons.Core.Contracts
{
    public interface IExternalService
    {
        Task<FacebookRegistrationResult> RegisterWithFacebook();
        Task<GoogleRegistrationResult> RegisterWithGoogle();
    }
}
