namespace EventApi.Utility
{
    using System.Security.Cryptography;

    public static class KeyGeneration
    {
        private static readonly HMACSHA256 Hmac = new HMACSHA256();

        public static string GetAlgorithm() => "HS256";

        public static byte[] GetKey() => Hmac.Key;
    }
}