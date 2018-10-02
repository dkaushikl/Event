namespace EventApi.Controllers
{
    using System;
    using System.Web.Http;

    using EventApi.Utility.JwtToken;

    public class BaseController : ApiController
    {
        public int? UserId => this.GetDefaultUser();

        private int? GetDefaultUser()
        {
            var request = this.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                return null;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                return null;
            }

            var token = authorization.Parameter;

            string userId;
            if (!JwtProvider.ValidateToken(token, out var email, out userId))
            {
                return null;
            }

            return Convert.ToInt32(userId);

            // var accessToken = data.Headers.Authorization.ToString();

            // if (string.IsNullOrEmpty(Convert.ToString(accessToken, CultureInfo.CurrentCulture)))
            // {
            // return 0;
            // }

            // var simplePrinciple = JwtProvider.GetPrincipal(accessToken);

            // if (!(simplePrinciple?.Identity is ClaimsIdentity identity))
            // {
            // return 0;
            // }

            // if (!identity.IsAuthenticated)
            // {
            // return 0;
            // }

            // var userIdClaim = identity.FindFirst(ClaimTypes.Name);
            // return Convert.ToInt32(userIdClaim?.Value);
        }
    }
}