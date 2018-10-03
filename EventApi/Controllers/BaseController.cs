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
        }
    }
}