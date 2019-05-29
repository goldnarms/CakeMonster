namespace Kakemons.Core.NavigationModels
{
    public class UserProfileNavigation
    {
        public string UserId { get; }
        public string AccessToken { get; }

        public UserProfileNavigation(string userId, string accessToken)
        {
            UserId = userId;
            AccessToken = accessToken;
        }
    }
}
