namespace commercetools.Sdk.HttpApi.Tests
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.HttpApi;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net.Http;
    using Xunit;

    public class Examples
    {
        // These are not real unit tests, but something like "integration tests" and a way to test code in a simple way

        [Fact]
        public void GetClientCredentialsToken()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("Client");
            // Resetting scope to an empty string for testing purposes
            clientConfiguration.Scope = "";
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            ISessionManager sessionManager = new MockSessionManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(httpClientFactory, clientConfiguration, sessionManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetClientCredentialsTokenWithScope()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("ClientWithSmallerScope");
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            ISessionManager sessionManager = new MockSessionManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(httpClientFactory, clientConfiguration, sessionManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
            Assert.Equal(clientConfiguration.Scope, token.Scope);
        }

        [Fact]
        public void GetPasswordToken()
        {
            // TODO Move configuration to a separate file
            IClientConfiguration clientConfiguration = getClientConfiguration("ClientWithSmallerScope");
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            ISessionManager sessionManager = new MockSessionManager();
            sessionManager.Username = "mick.jagger@commercetools.com";
            sessionManager.Password = "st54e9m4";
            sessionManager.TokenFlow = TokenFlow.Password;
            ITokenProvider tokenProvider = new PasswordTokenProvider(httpClientFactory, clientConfiguration, sessionManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
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
        public void GetCategoryByIdSingleClientCredentials()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("Client");
            ISessionManager sessionManager = new MockSessionManager();
            IHttpClientFactory httpClientFactoryAuth = new MockHttpClientFactory(null);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactoryAuth, clientConfiguration, sessionManager);
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(clientCredentialsTokenProvider);
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(authorizationHandler);

            IClient commerceToolsClient = new Client(httpClientFactory, clientConfiguration);
            string categoryId = "f40fcd15-b1c2-4279-9cfa-f6083e6a2988";
            Category category = commerceToolsClient.GetCategoryById(new Guid(categoryId));
            Assert.Equal(categoryId, category.Id.ToString());
        }

        private ClientConfiguration getClientConfiguration(string clientSettings)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return config.GetSection(clientSettings).Get<ClientConfiguration>();
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
                    this.authorizationHandler.InnerHandler = new HttpClientHandler();
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