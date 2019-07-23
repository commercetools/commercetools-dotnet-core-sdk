namespace commercetools.Sdk.HttpApi.IntegrationTests.Tokens
{
    using commercetools.Sdk.HttpApi;
    using commercetools.Sdk.HttpApi.Domain;
    using commercetools.Sdk.HttpApi.Tokens;
    using commercetools.Sdk.Serialization;
    using System.Net.Http;
    using Xunit;

    [Collection("Integration Tests")]
    public class TokenIntegrationTests
    {
        private readonly ClientFixture clientFixture;

        public TokenIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.clientFixture = new ClientFixture(serviceProviderFixture);
        }

        [Fact]
        public void GetClientCredentialsToken()
        {
            IClientConfiguration clientConfiguration = this.clientFixture.GetClientConfiguration("TokenClient");
            // Resetting scope to an empty string for testing purposes
            clientConfiguration.Scope = "";
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(
                this.clientFixture.GetService<IHttpClientFactory>(),
                tokenStoreManager,
                this.clientFixture.GetService<ISerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetClientCredentialsTokenWithScope()
        {
            IClientConfiguration clientConfiguration = this.clientFixture.GetClientConfiguration("TokenClientWithSmallerScope");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(
                this.clientFixture.GetService<IHttpClientFactory>(),
                tokenStoreManager,
                this.clientFixture.GetService<ISerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
            Assert.Equal(clientConfiguration.Scope, token.Scope);
        }

        [Fact(Skip = "Depends on the project user which might not be set.")]
        public void GetPasswordToken()
        {
            IClientConfiguration clientConfiguration = this.clientFixture.GetClientConfiguration("TokenClientWithSmallerScope");
            InMemoryUserCredentialsStoreManager userCredentialsStoreManager = new InMemoryUserCredentialsStoreManager();
            userCredentialsStoreManager.Username = "mick.jagger@commercetools.com";
            userCredentialsStoreManager.Password = "st54e9m4";
            ITokenProvider tokenProvider = new PasswordTokenProvider(
                this.clientFixture.GetService<IHttpClientFactory>(),
                userCredentialsStoreManager,
                this.clientFixture.GetService<ISerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetAnonymousTokenNoIdProvided()
        {
            IClientConfiguration clientConfiguration = this.clientFixture.GetClientConfiguration("TokenClientWithAnonymousScope");
            IAnonymousCredentialsStoreManager anonymousStoreManager = new InMemoryAnonymousCredentialsStoreManager();
            ITokenProvider tokenProvider = new AnonymousSessionTokenProvider(
                this.clientFixture.GetService<IHttpClientFactory>(),
                anonymousStoreManager,
                this.clientFixture.GetService<ISerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetAnonymousTokenIdProvided()
        {
            IClientConfiguration clientConfiguration = this.clientFixture.GetClientConfiguration("TokenClientWithAnonymousScope");
            InMemoryAnonymousCredentialsStoreManager anonymousStoreManager = new InMemoryAnonymousCredentialsStoreManager();
            anonymousStoreManager.AnonymousId = TestingUtility.RandomString(10);
            ITokenProvider tokenProvider = new AnonymousSessionTokenProvider(
                this.clientFixture.GetService<IHttpClientFactory>(),
                anonymousStoreManager,
                this.clientFixture.GetService<ISerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact(Skip = "run only when fully enable appveyor and travis builds against the dedicated project of this build")]
        public void RefreshTokenPasswordFlow()
        {
            IClientConfiguration clientConfiguration = this.clientFixture.GetClientConfiguration("TokenClientWithSmallerScope");
            InMemoryUserCredentialsStoreManager userCredentialsStoreManager = new InMemoryUserCredentialsStoreManager();
            userCredentialsStoreManager.Username = "mick.jagger@commercetools.com";
            userCredentialsStoreManager.Password = "st54e9m4";
            ITokenProvider tokenProvider = new PasswordTokenProvider(
                this.clientFixture.GetService<IHttpClientFactory>(),
                userCredentialsStoreManager,
                this.clientFixture.GetService<ISerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            string initialAccessToken = token.AccessToken;
            // TODO Find a better way to test this (with mock objects perhaps)
            token.ExpiresIn = 0;
            token = tokenProvider.Token;
            Assert.NotEqual(token.AccessToken, initialAccessToken);
        }
    }
}
