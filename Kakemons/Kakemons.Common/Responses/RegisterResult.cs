using Kakemons.Common.Dtos;

namespace Kakemons.Common.Responses
{
    public class RegisterResult
    {
        public bool IsSuccessful { get; set; }
        public UserDto User { get; set; }
    }
}
