using System.Text.RegularExpressions;

namespace Infrastructure.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveEscapeSequences(this string content)
        {
            return Regex.Replace(content, @"\t|\n|\r", "");
        }

        public static string EscapeSpaces(this string content)
        {
            return content.Replace(" ", "\\ ");
        }
    }
}
