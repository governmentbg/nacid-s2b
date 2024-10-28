using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers.ValidateProperties
{
    public static class ValidatePropertiesHelper
    {
        public static bool IsValidCyrillicName(string value)
        {
            return new Regex(@"^[А-Яа-я-'\s]+$").IsMatch(value);
        }

        public static bool IsValidLatinName(string value)
        {
            return new Regex(@"^[A-Za-z-'\s]+$").IsMatch(value);
        }

        public static bool IsValidEmail(string value)
        {
            return new EmailAddressAttribute().IsValid(value);
        }

        public static bool IsValidPhoneNumber(string value)
        {
            return new Regex(@"^\+\d{1,12}$").IsMatch(value) || (value.Length > 8 && value.Length < 19 && new Regex(@"^[0-9-\s\+]+$").IsMatch(value));
        }

        public static bool IsValidCyrillic(string value)
        {
            return new Regex(@"^[А-Яа-я0-9-VIX№.,""„“\s]+$").IsMatch(value);
        }

        public static bool IsValidLatin(string value)
        {
            return new Regex(@"^[A-Za-z0-9-#№.,""„“\s]+$").IsMatch(value);
        }

        public static bool IsDigitsOnly(string value)
        {
            return value.All(char.IsDigit);
        }
    }
}
