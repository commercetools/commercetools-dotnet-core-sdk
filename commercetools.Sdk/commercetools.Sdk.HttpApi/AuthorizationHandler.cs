namespace commercetools.Sdk.HttpApi
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class AuthorizationHandler : DelegatingHandler
    {
        private ITokenProviderFactory tokenProviderFactory;
        private ITokenFlowRegister tokenFlowRegister;

        public AuthorizationHandler(ITokenProviderFactory tokenProviderFactory, ITokenFlowRegister tokenFlowRegister)
        {
            this.tokenProviderFactory = tokenProviderFactory;
            this.tokenFlowRegister = tokenFlowRegister;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ITokenProvider tokenProvider = this.tokenProviderFactory.GetTokenProviderByFlow(this.tokenFlowRegister.TokenFlow);
            Token token = tokenProvider.Token;
            request.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}