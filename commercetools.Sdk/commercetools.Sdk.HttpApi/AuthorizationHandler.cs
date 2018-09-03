namespace commercetools.Sdk.HttpApi
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class AuthorizationHandler : DelegatingHandler
    {
        private ISessionManager sessionManager;
        private ITokenProviderFactory tokenProviderFactory;

        public AuthorizationHandler(ISessionManager sessionManager, ITokenProviderFactory tokenProviderFactory)
        {
            this.sessionManager = sessionManager;
            this.tokenProviderFactory = tokenProviderFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            TokenFlow tokenFlow = this.sessionManager.TokenFlow;
            ITokenProvider tokenProvider = this.tokenProviderFactory.GetTokenProviderByFlow(tokenFlow);
            Token token = tokenProvider.Token;
            request.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}