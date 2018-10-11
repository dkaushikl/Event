namespace EventApi.Utility.JwtToken
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            this.ReasonPhrase = reasonPhrase;
            this.Request = request;
        }

        public string ReasonPhrase { get; }

        public HttpRequestMessage Request { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Execute());
        }

        private HttpResponseMessage Execute()
        {
            var response =
                new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        RequestMessage = this.Request,
                        ReasonPhrase = this.ReasonPhrase
                    };

            return response;
        }
    }
}