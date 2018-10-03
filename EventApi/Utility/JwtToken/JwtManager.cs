namespace EventApi.Utility.JwtToken
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;

    using Microsoft.IdentityModel.Tokens;

    public static class JwtProvider
    {
        private static string secret;

        private static string Secret
        {
            get
            {
                if (secret != null)
                {
                    return secret;
                }

                var hmac = new HMACSHA256();
                secret = Convert.ToBase64String(hmac.Key);

                return secret;
            }
        }

        public static string GenerateToken(string email, string userId, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject =
                                              new ClaimsIdentity(
                                                  new[]
                                                      {
                                                          new Claim(ClaimTypes.Name, userId),
                                                          new Claim(ClaimTypes.Email, email)
                                                      }),
                Expires =
                                              now.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = new SigningCredentials(
                                              new SymmetricSecurityKey(symmetricKey),
                                              SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public static bool ValidateToken(string token, out string email, out string userId)
        {
            email = null;
            userId = null;

            var simplePrinciple = GetPrincipal(token);

            if (!(simplePrinciple?.Identity is ClaimsIdentity identity))
            {
                return false;
            }

            if (!identity.IsAuthenticated)
            {
                return false;
            }

            var emailClaim = identity.FindFirst(ClaimTypes.Email);
            email = emailClaim?.Value;

            var userIdClaim = identity.FindFirst(ClaimTypes.Name);
            userId = userIdClaim?.Value;

            return !string.IsNullOrEmpty(email);
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (!(tokenHandler.ReadToken(token) is JwtSecurityToken))
                {
                    return null;
                }

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey =
                                                       new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}