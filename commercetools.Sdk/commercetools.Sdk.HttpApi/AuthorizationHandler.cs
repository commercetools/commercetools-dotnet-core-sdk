namespace commercetools.Sdk.HttpApi
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class AuthorizationHandler : DelegatingHandler
    {
        private ITokenProvider tokenProvider;

        public AuthorizationHandler(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Token token = tokenProvider.Token;
            request.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}