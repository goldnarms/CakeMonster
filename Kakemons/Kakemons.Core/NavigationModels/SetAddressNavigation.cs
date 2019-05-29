namespace Kakemons.Core.NavigationModels
{
    public class SetAddressNavigation
    {
        public string UserId { get; }
        public string AccessToken { get; }
        public string Firstname { get; }
        public string Lastname { get; }

        public SetAddressNavigation(string userId, string accessToken, string firstname, string lastname)
        {
            UserId = userId;
            AccessToken = accessToken;
            Firstname = firstname;
            Lastname = lastname;
        }
    }
}
