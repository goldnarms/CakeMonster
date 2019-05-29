namespace Kakemons.Common.Responses
{
    public class LoginUserResponse
    {
        public bool IsSuccessful { get; }
        public string AccessToken { get; }
        public string UserId { get; }

        public LoginUserResponse(bool isSuccessful, string accessToken, string userId)
        {
            IsSuccessful = isSuccessful;
            AccessToken = accessToken;
            UserId = userId;
        }
    }
}
