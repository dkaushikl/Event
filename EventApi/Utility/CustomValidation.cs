namespace EventApi.Utility
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public static class CustomValidation
    {
        public static bool IsEmail(this string email)
        {
            return Regex.IsMatch(
                email,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);
        }

        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsMobileNo(this string mobileNo)
        {
            return mobileNo != null && Regex.IsMatch(mobileNo, @"\+?[0-9]{10}");
        }

        public static bool IsPassword(this string password)
        {
            return password.Length >= 6 && password.Length <= 15;
        }

        public static bool IsValidDateFormat(this string objDate)
        {
            return DateTime.TryParseExact(
                objDate,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var _);
        }

        public static bool IsValidTimeFormat(this string input)
        {
            return TimeSpan.TryParse(input, out var _);
        }
    }
}