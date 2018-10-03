namespace EventApi.Utility
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class CustomValidation
    {
        public static async Task<bool> IsEmail(this string email)
        {
            var result = await Task.Run(
                             () =>
                                 {
                                     if (Regex.IsMatch(
                                             email,
                                             @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))"
                                             + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                                             RegexOptions.IgnoreCase,
                                             TimeSpan.FromMilliseconds(250)) && !string.IsNullOrEmpty(email))
                                     {
                                         return true;
                                     }

                                     return false;
                                 });
            return result;
        }
    }
}