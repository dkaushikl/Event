namespace EventApi.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.IdentityModel.Tokens;

    internal class TokenValidation : DelegatingHandler
    {
        private const string WebUrl = "http://localhost:50554";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token;
            //determine whether a jwt exists or not
            if (!TryRetrieveAuthToken(request, out token))
            {
                //allow requests with no token - whether a action method needs an authentication can be set with the claims authorization attribute
                return base.SendAsync(request, cancellationToken);
            }

            HttpResponseMessage responseMessage;
            try
            {
                //extract and assign the user of the jwt
                var claimsPrincipal = GetPrincipal(token, LifetimeValidator);
                Thread.CurrentPrincipal = claimsPrincipal;
                HttpContext.Current.User = claimsPrincipal;

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenExpiredException tokenExpiredException)
            {
                responseMessage = request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized,
                    $"Auth token validation error: {tokenExpiredException}",
                    tokenExpiredException);
            }
            catch (SecurityTokenValidationException tokenValidationException)
            {
                responseMessage = request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized,
                    $"Auth token validation error: {tokenValidationException.Message}",
                    tokenValidationException);
            }
            catch (Exception exception)
            {
                responseMessage = request.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    exception);
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage, cancellationToken);
        }

        private bool LifetimeValidator(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            if (expires == null)
                return false;

            if (DateTime.UtcNow > expires)
                throw new SecurityTokenExpiredException("Token expired!");

            return true;
        }

        public static bool TryRetrieveAuthToken(HttpRequestMessage request, out string token)
        {
            token = null;
            request.Headers.TryGetValues("Authorization", out IEnumerable<string> authorizationHeaders);

            var authHeaders = authorizationHeaders?.ToList();
            if (authHeaders == null || authHeaders.Count > 1)
                return false;

            var bearerToken = authHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        public static bool TryRetrieveRefreshToken(HttpRequestMessage request, out string refreshToken)
        {
            refreshToken = null;
            request.Headers.TryGetValues("Refresh", out IEnumerable<string> authorizationHeaders);

            var refreshHeaders = authorizationHeaders?.ToList();
            if (refreshHeaders == null || refreshHeaders.Count > 1)
                return false;

            refreshToken = refreshHeaders.ElementAt(0);
            return true;
        }

        public static IPrincipal GetPrincipal(string token, LifetimeValidator lifetimeValidator)
        {
            const string secKeyToken =
                "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secKeyToken));
            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = WebUrl,
                ValidIssuer = WebUrl,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = lifetimeValidator,
                IssuerSigningKey = securityKey
            };

            //extract and assign the user of the jwt
            return new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out SecurityToken securityToken);
        }
    }
}