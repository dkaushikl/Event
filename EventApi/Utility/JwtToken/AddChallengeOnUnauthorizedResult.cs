namespace EventApi.Utility.JwtToken
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            this.Challenge = challenge;
            this.InnerResult = innerResult;
        }

        public AuthenticationHeaderValue Challenge { get; }

        public IHttpActionResult InnerResult { get; }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await this.InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                if (response.Headers.WwwAuthenticate.All(h => h.Scheme != this.Challenge.Scheme))
                    response.Headers.WwwAuthenticate.Add(this.Challenge);

            return response;
        }
    }
}