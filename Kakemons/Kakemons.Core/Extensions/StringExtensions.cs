namespace Kakemons.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToFirstCaseUpper(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
