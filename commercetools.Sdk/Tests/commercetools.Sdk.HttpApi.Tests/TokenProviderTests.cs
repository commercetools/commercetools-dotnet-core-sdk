using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.HttpApi.Tokens;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TokenProviderTests : IClassFixture<ClientFixture>
    {
        private readonly ClientFixture clientFixture;

        public TokenProviderTests(ClientFixture clientFixture)
        {
            this.clientFixture = clientFixture;
        }

        [Fact]
        public void GetTokenUsingClientCredentialsTokenProvider()
        {
            //Arrange

            //Create httpClientFactory which return Token as json response
            var mockHttpClientFactory =
                this.clientFixture.GetClientFactoryMockWithSpecificResponse(DefaultClientNames.Authorization,
                    "Resources/Responses/GetToken.json");

            var tokenStoreManager = this.clientFixture.GetService<ITokenStoreManager>();

            var clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(
                mockHttpClientFactory.Object,
                tokenStoreManager,
                this.clientFixture.GetService<ITokenSerializerService>()
            );
            clientCredentialsTokenProvider.ClientConfiguration = this.clientFixture.GetClientConfiguration("Client");

            //Assert
            Assert.Equal(TokenFlow.ClientCredentials, clientCredentialsTokenProvider.TokenFlow);
            Assert.NotNull(clientCredentialsTokenProvider.Token);
            Assert.NotNull(tokenStoreManager.Token);
            Assert.Null(tokenStoreManager.Token.RefreshToken);
        }

        /// <summary>
        /// Get Refresh Token if tokenStoreManager Already have Expired Token with RefreshToken
        /// </summary>
        [Fact]
        public void GetRefreshTokenUsingClientCredentialsTokenProvider()
        {
            //Arrange

            //Create httpClientFactory which return Token as json response with refresh token
            var mockHttpClientFactory =
                this.clientFixture.GetClientFactoryMockWithSpecificResponse(DefaultClientNames.Authorization,
                    "Resources/Responses/GetNewTokenWithRefresh.json");

            //Create tokenStoreManager with Expired Token
            var token = new Token
            {
                AccessToken = this.clientFixture.RandomString(10),
                RefreshToken = this.clientFixture.RandomString(10),
                ExpiresIn = -172800 //just to make it expired with true
            };
            var tokenStoreManager = new InMemoryTokenStoreManager
            {
                Token = token
            };

            //Create ClientCredentialsTokenProvider
            var clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(
                mockHttpClientFactory.Object,
                tokenStoreManager,
                this.clientFixture.GetService<ITokenSerializerService>()
            );
            clientCredentialsTokenProvider.ClientConfiguration = this.clientFixture.GetClientConfiguration("Client");

            //Assert
            Assert.True(tokenStoreManager.Token.Expired);
            Assert.NotNull(tokenStoreManager.Token.RefreshToken);
            Assert.Equal(TokenFlow.ClientCredentials, clientCredentialsTokenProvider.TokenFlow);

            //Get the new token using the refresh token, new token will be saved in tokenStoreManager.Token
            Assert.NotNull(clientCredentialsTokenProvider.Token);
            Assert.False(tokenStoreManager.Token.Expired);
            Assert.NotEqual(token.AccessToken, tokenStoreManager.Token.AccessToken);
            Assert.NotEqual(token.RefreshToken, tokenStoreManager.Token.RefreshToken);
            Assert.True(tokenStoreManager.Token.ExpiresIn > 172800); // the new token expired in 6 month not 2 days

        }

        /// <summary>
        /// Get new Token if tokenStoreManager Already have Expired Token without RefreshToken
        /// </summary>
        [Fact]
        public void GetNewTokenUsingClientCredentialsTokenProvider()
        {
            //Arrange

            //Create httpClientFactory which return Token as json response with refresh token
            var mockHttpClientFactory =
                this.clientFixture.GetClientFactoryMockWithSpecificResponse(DefaultClientNames.Authorization,
                    "Resources/Responses/GetToken.json");

            //Create tokenStoreManager with Expired Token and without Refresh token
            var token = new Token
            {
                AccessToken = this.clientFixture.RandomString(10),
                ExpiresIn = -172800 //just to make it expired with true
            };
            var tokenStoreManager = new InMemoryTokenStoreManager
            {
                Token = token
            };

            //Create ClientCredentialsTokenProvider
            var clientCredentialsTokenProvider = new ClientCredentialsTokenProvider(
                mockHttpClientFactory.Object,
                tokenStoreManager,
                this.clientFixture.GetService<ITokenSerializerService>()
            );
            clientCredentialsTokenProvider.ClientConfiguration = this.clientFixture.GetClientConfiguration("Client");

            //Assert
            Assert.True(tokenStoreManager.Token.Expired);
            Assert.True(string.IsNullOrEmpty(tokenStoreManager.Token.RefreshToken));
            Assert.Equal(TokenFlow.ClientCredentials, clientCredentialsTokenProvider.TokenFlow);

            //Get the new token, new token will be saved in tokenStoreManager.Token
            Assert.NotNull(clientCredentialsTokenProvider.Token);
            Assert.False(tokenStoreManager.Token.Expired);
            Assert.NotEqual(token.AccessToken, tokenStoreManager.Token.AccessToken);
            Assert.True(tokenStoreManager.Token.ExpiresIn == 172800); // the new token expired in 2 days

        }
    }
}
