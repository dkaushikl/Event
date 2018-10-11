namespace Event.Core
{
    using Newtonsoft.Json;

    public class RegisterViewModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("mobileno")]
        public string Mobile { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class AccountViewModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}