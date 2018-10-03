namespace EventApi.Utility.JwtToken
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(object jsonContent, HttpRequestMessage request)
        {
            this.JsonContent = jsonContent;
            this.Request = request;
        }

        public HttpRequestMessage Request { get; }

        public object JsonContent { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) => Task.FromResult(this.Execute());

        private HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = this.Request,
                Content = new ObjectContent(
                            this.JsonContent.GetType(),
                            this.JsonContent,
                            new JsonMediaTypeFormatter())
            };
            return response;
        }
    }
}