using System.Threading.Tasks;
using Kakemons.Common.Responses;
using Kakemons.Core.Contracts;

namespace Kakemons.Core.Fakes
{
    public class FakeExternalService : IExternalService
    {
        public Task<FacebookRegistrationResult> RegisterWithFacebook()
        {
            return Task.FromResult(new FacebookRegistrationResult
            {
                AccessToken = "",
                UserId = ""
            });
        }

        public Task<GoogleRegistrationResult> RegisterWithGoogle()
        {
            return Task.FromResult(new GoogleRegistrationResult
            {
                AccessToken = "",
                UserId = ""
            });
        }
    }
}
