using Kakemons.Common.Responses;

namespace Kakemons.Common.Parameters
{
    public class RegisterUserParameter
    {
        private string _firstname;
        private string _lastname;
        private GeoCodeResult _address;

        public RegisterUserParameter(string firstname, string lastname, GeoCodeResult address)
        {
            _firstname = firstname;
            _lastname = lastname;
            _address = address;
        }
    }
}
