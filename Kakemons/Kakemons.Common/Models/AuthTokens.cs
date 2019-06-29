namespace Kakemons.Common.Models
{
    public class AuthTokens
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
    }
}