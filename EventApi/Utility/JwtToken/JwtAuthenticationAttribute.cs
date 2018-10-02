namespace EventApi.Utility.JwtToken
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;

    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public string Email { get; set; }

        public string Realm { get; set; }

        public string UserId { get; set; }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await this.AuthenticateJwtToken(token);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            this.Challenge(context);
            return Task.FromResult(0);
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            string email, userId;

            if (!JwtProvider.ValidateToken(token, out email, out userId))
            {
                return Task.FromResult<IPrincipal>(null);
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userId), new Claim(ClaimTypes.Email, email) };
            var identity = new ClaimsIdentity(claims, "Jwt");
            IPrincipal user = new ClaimsPrincipal(identity);
            return Task.FromResult(user);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(this.Realm))
            {
                parameter = "realm=\"" + this.Realm + "\"";
            }

            context.ChallengeWith("Bearer", parameter);
        }
    }
}