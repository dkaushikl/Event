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

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                return;
            }

            const string ErrorMessage = "The account being accessed does not have sufficient permissions to execute this operation.";

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult(new { Error = true, Message = ErrorMessage }, request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await this.AuthenticateJwtToken(token);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(new { Error = true, Message = ErrorMessage }, request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken) => Task.FromResult(0);

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
    }
}