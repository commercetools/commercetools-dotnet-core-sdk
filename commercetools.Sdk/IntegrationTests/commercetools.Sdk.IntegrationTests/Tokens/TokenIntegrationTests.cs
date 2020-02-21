using Microsoft.Extensions.DependencyInjection;
using commercetools.Sdk.HttpApi;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.HttpApi.Tokens;
using commercetools.Sdk.Serialization;
using System.Net.Http;
using Xunit;

namespace commercetools.Sdk.IntegrationTests.Tokens
{
    [Collection("Integration Tests")]
    public class TokenIntegrationTests
    {
        private readonly ServiceProviderFixture serviceProviderFixture;

        private readonly ServiceProvider provider;

        public TokenIntegrationTests(ServiceProviderFixture serviceProviderFixture)
        {
            this.serviceProviderFixture = serviceProviderFixture;
            var collection = new ServiceCollection();
            collection.AddHttpClient(DefaultClientNames.Authorization);
            this.provider = collection.BuildServiceProvider();

        }

        [Fact]
        public void GetClientCredentialsToken()
        {
            IClientConfiguration clientConfiguration = this.serviceProviderFixture.GetClientConfiguration("TokenClient");
            // Resetting scope to an empty string for testing purposes
            clientConfiguration.Scope = "";
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(
                this.provider.GetService<IHttpClientFactory>(),
                tokenStoreManager,
                this.serviceProviderFixture.GetService<ITokenSerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetClientCredentialsTokenWithScope()
        {
            IClientConfiguration clientConfiguration = this.serviceProviderFixture.GetClientConfiguration("TokenClientWithSmallerScope");
            ITokenStoreManager tokenStoreManager = new InMemoryTokenStoreManager();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(
                this.provider.GetService<IHttpClientFactory>(),
                tokenStoreManager,
                this.serviceProviderFixture.GetService<ITokenSerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact(Skip = "Depends on the project user which might not be set.")]
        public void GetPasswordToken()
        {
            IClientConfiguration clientConfiguration = this.serviceProviderFixture.GetClientConfiguration("TokenClientWithSmallerScope");
            InMemoryUserCredentialsStoreManager userCredentialsStoreManager = new InMemoryUserCredentialsStoreManager();
            userCredentialsStoreManager.Username = "mick.jagger@commercetools.com";
            userCredentialsStoreManager.Password = "st54e9m4";
            ITokenProvider tokenProvider = new PasswordTokenProvider(
                this.provider.GetService<IHttpClientFactory>(),
                userCredentialsStoreManager,
                this.serviceProviderFixture.GetService<ITokenSerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetAnonymousTokenNoIdProvided()
        {
            IClientConfiguration clientConfiguration = this.serviceProviderFixture.GetClientConfiguration("TokenClientWithAnonymousScope");
            IAnonymousCredentialsStoreManager anonymousStoreManager = new InMemoryAnonymousCredentialsStoreManager();
            ITokenProvider tokenProvider = new AnonymousSessionTokenProvider(
                this.provider.GetService<IHttpClientFactory>(),
                anonymousStoreManager,
                this.serviceProviderFixture.GetService<ITokenSerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetAnonymousTokenIdProvided()
        {
            IClientConfiguration clientConfiguration = this.serviceProviderFixture.GetClientConfiguration("TokenClientWithAnonymousScope");
            InMemoryAnonymousCredentialsStoreManager anonymousStoreManager = new InMemoryAnonymousCredentialsStoreManager();
            anonymousStoreManager.AnonymousId = TestingUtility.RandomString(10);
            ITokenProvider tokenProvider = new AnonymousSessionTokenProvider(
                this.provider.GetService<IHttpClientFactory>(),
                anonymousStoreManager,
                this.serviceProviderFixture.GetService<ITokenSerializerService>());
            tokenProvider.ClientConfiguration = clientConfiguration;
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact(Skip = "run only when fully enable appveyor and travis builds against the dedicated project of this build")]
        public void RefreshTokenPasswordFlow()
        {
            IClientConfiguration clientConfiguration = this.serviceProviderFixture.GetClientConfiguration("TokenClientWithSmallerScope");
            InMemoryUserCredentialsStoreManager userCredentialsStoreManager = new InMemoryUserCredentialsStoreManager();
            userCredentialsStoreManager.Username = "mick.jagger@commercetools.com";
            userCredentialsStoreManager.Password = "st54e9m4";
            ITokenProvider tokenProvider = new PasswordTokenProvider(
                this.provider.GetService<IHttpClientFactory>(),
                userCredentialsStoreManager,
                this.serviceProviderFixture.GetService<ITokenSerializerService>());
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
