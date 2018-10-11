namespace EventApi.Utility
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public static class CustomValidation
    {
        public static bool IsEmail(this string email) => Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

        public static bool IsMobileNo(this string mobileNo) => mobileNo != null && Regex.IsMatch(mobileNo, @"\+?[0-9]{10}");

        public static bool IsEmpty(this string str) => string.IsNullOrEmpty(str);

        public static bool IsPassword(this string password) => password.Length >= 6 && password.Length <= 15;

        public static bool IsValidTimeFormat(this string input) => TimeSpan.TryParse(input, out var _);

        public static bool IsValidDateFormat(this string objDate) => DateTime.TryParseExact(objDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _);
    }
}