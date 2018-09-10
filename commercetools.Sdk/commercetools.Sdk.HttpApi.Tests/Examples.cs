namespace commercetools.Sdk.HttpApi.Tests
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.HttpApi;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Xunit;

    // TODO Thing of better names for tests
    // These are not real unit tests, but something like "integration tests" and a way to test code in a simple way
    public class Examples
    {      
        [Fact]
        public void GetClientCredentialsToken()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("Client");
            // Resetting scope to an empty string for testing purposes
            clientConfiguration.Scope = "";
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(httpClientFactory, clientConfiguration, tokenStoreManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetClientCredentialsTokenWithScope()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("ClientWithSmallerScope");
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(httpClientFactory, clientConfiguration, tokenStoreManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
            Assert.Equal(clientConfiguration.Scope, token.Scope);
        }

        [Fact]
        public void GetPasswordToken()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("ClientWithSmallerScope");
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            IUserCredentialsStoreManager userCredentialsStoreManager = new InMemoryUserCredentialsStoreManager();
            userCredentialsStoreManager.Username = "mick.jagger@commercetools.com";
            userCredentialsStoreManager.Password = "st54e9m4";
            ITokenProvider tokenProvider = new PasswordTokenProvider(httpClientFactory, clientConfiguration, userCredentialsStoreManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetAnonymousTokenNoIdProvided()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("ClientWithAnonymousScope");
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            IAnonymousCredentialsStoreManager anonymousStoreManager = new InMemoryAnonymousCredentialsStoreManager();
            ITokenProvider tokenProvider = new AnonymousSessionTokenProvider(httpClientFactory, clientConfiguration, anonymousStoreManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetAnonymousTokenIdProvided()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("ClientWithAnonymousScope");
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            IAnonymousCredentialsStoreManager anonymousStoreManager = new InMemoryAnonymousCredentialsStoreManager();
            anonymousStoreManager.AnonymousId = RandomString(10);
            ITokenProvider tokenProvider = new AnonymousSessionTokenProvider(httpClientFactory, clientConfiguration, anonymousStoreManager);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void RefreshTokenPasswordFlow()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("ClientWithSmallerScope");
            IHttpClientFactory httpClientFactory = new MockHttpClientFactory(null);
            IUserCredentialsStoreManager userCredentialsStoreManager = new InMemoryUserCredentialsStoreManager();
            userCredentialsStoreManager.Username = "mick.jagger@commercetools.com";
            userCredentialsStoreManager.Password = "st54e9m4";
            ITokenProvider tokenProvider = new PasswordTokenProvider(httpClientFactory, clientConfiguration, userCredentialsStoreManager);
            Token token = tokenProvider.Token;
            string initialAccessToken = token.AccessToken;
            // TODO Find a better way to test this (with mock objects perhaps)
            token.ExpiresIn = 0;
            token = tokenProvider.Token;
            Assert.NotEqual(token.AccessToken, initialAccessToken);
        }

        [Fact]
        public void RegisterTokenFlows()
        {
            // TODO Remove this tests since now it is very simple and there is no business logic
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            Assert.Equal(TokenFlow.ClientCredentials, tokenFlowRegister.TokenFlow);
        }

        [Fact]
        public void GetCategoryByIdSingleClientCredentials()
        {
            IClientConfiguration clientConfiguration = getClientConfiguration("Client");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            IHttpClientFactory httpClientFactoryAuth = new MockHttpClientFactory(null);
            ITokenProvider clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(httpClientFactoryAuth, clientConfiguration, tokenStoreManager);
            ITokenFlowRegister tokenFlowRegister = new InMemoryTokenFlowRegister();
            tokenFlowRegister.TokenFlow = TokenFlow.ClientCredentials;
            ITokenProviderFactory tokenProviderFactory = new TokenProviderFactory(new List<ITokenProvider>() { clientCredentialsTokenProvider });
            AuthorizationHandler authorizationHandler = new AuthorizationHandler(tokenProviderFactory, tokenFlowRegister);
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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}