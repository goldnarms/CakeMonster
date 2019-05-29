using Kakemons.Common.Responses;
using Kakemons.Core.ViewModels.Register;

namespace Kakemons.Core.NavigationModels
{
    public class DisclaimerNavigation
    {
        public string UserId { get; }
        public string AccessToken { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public GeoCodeResult AddressResult { get; }

        public DisclaimerNavigation(string userId, string accessToken, string firstname, string lastname, GeoCodeResult addressResult)
        {
            UserId = userId;
            AccessToken = accessToken;
            Firstname = firstname;
            Lastname = lastname;
            AddressResult = addressResult;
        }
    }
}
