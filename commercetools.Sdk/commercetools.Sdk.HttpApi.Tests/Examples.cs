namespace commercetools.Sdk.HttpApi.Tests
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.HttpApi;
    using System.Net.Http;
    using Xunit;

    public class Examples
    {
        // These are not real unit tests, but something like "integration tests" and a way to test code in a simple way

        [Fact]
        public void GetClientCredentialsToken()
        {
            // TODO Move configuration to a separate file
            IClientConfiguration clientConfiguration = new ClientConfiguration();
            clientConfiguration.ClientId = "pV7ogtY4wPRWbqQUQJ5TBWmh";
            clientConfiguration.ClientSecret = "s-mSeiIojSUUgiht5kAA_7cLvaxXrMl6";
            // this is the only scope defined on the client
            //clientConfiguration.Scope = "manage_project:portablevendor";
            clientConfiguration.AuthorizationBaseAddress = "https://auth.sphere.io/";
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            ISessionManager sessionManager = new MockSessionManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(httpClientFactory, clientConfiguration, sessionManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetClientCredentialsTokenWithScope()
        {
            IClientConfiguration clientConfiguration = new ClientConfiguration();
            clientConfiguration.ClientId = "jBRiUPK2i9BuavWFsNZtyZt2";
            clientConfiguration.ClientSecret = "5A-sGKns7k8h5APKCBTmGb60DjMYOG3j";
            clientConfiguration.Scope = "view_products:portablevendor";
            clientConfiguration.AuthorizationBaseAddress = "https://auth.sphere.io/";
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            ISessionManager sessionManager = new MockSessionManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(httpClientFactory, clientConfiguration, sessionManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
            Assert.Equal(clientConfiguration.Scope, token.Scope);
        }

        [Fact]
        public void RegisterTokenFlows()
        {
            ITokenFlowRegister tokenFlowRegister = new TokenFlowRegister();
            tokenFlowRegister.RegisterFlow("client1", TokenFlow.ClientCredentials);
            tokenFlowRegister.RegisterFlow("client2", TokenFlow.Password);
            TokenFlow tokenFlowForClient1 = tokenFlowRegister.GetFlow("client1");
            Assert.Equal(TokenFlow.ClientCredentials, tokenFlowForClient1);
        }

        [Fact]
        public void ClientGeneration()
        {
            IClientConfiguration clientConfiguration = new ClientConfiguration();
            ITokenProviderFactory tokenProviderFactory = new TokenProviderFactory();
            ISessionManager sessionManager = new MockSessionManager();
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(sessionManager, tokenProviderFactory);
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(authorizationHandler);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactory, clientConfiguration, sessionManager);
            ITokenProvider passwordTokenProvider = new PasswordTokenProvider(httpClientFactory, clientConfiguration, sessionManager);
            ITokenProvider anonymousTokenProvider = new AnonymousSessionTokenProvider(httpClientFactory, clientConfiguration, sessionManager);
            tokenProviderFactory.RegisterTokenProvider(clientCredentialsTokenProvider);
            tokenProviderFactory.RegisterTokenProvider(passwordTokenProvider);
            tokenProviderFactory.RegisterTokenProvider(anonymousTokenProvider);

            sessionManager.Username = "bob";
            sessionManager.Password = "password";
            sessionManager.TokenFlow = TokenFlow.Password;

            IClient commerceToolsClient = new Client(httpClientFactory);
        }

        public class MockHttpClientFactory : IHttpClientFactory
        {
            private AuthorizationHandler authorizationHandler;

            public MockHttpClientFactory(AuthorizationHandler authorizationHandler)
            {
                this.authorizationHandler = authorizationHandler;
            }

            public HttpClient CreateClient(string name)
            {
                if (name == "api")
                {
                    HttpClient client = new HttpClient(this.authorizationHandler);
                    return client;
                }
                return new HttpClient();
            }
        }

        public class MockSessionManager : ISessionManager
        {
            public string ClientName { get; set; }
            public Token Token { get; set; }
            public TokenFlow TokenFlow { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string AnonymousId { get; set; }
        }
    }
}