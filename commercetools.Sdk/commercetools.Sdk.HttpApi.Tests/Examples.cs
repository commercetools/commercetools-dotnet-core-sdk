namespace commercetools.Sdk.HttpApi.Tests
{
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
            clientConfiguration.ClientSecret = "";
            // this is the only scope defined on the client
            //clientConfiguration.Scope = "manage_project:portablevendor";
            clientConfiguration.AuthorizationBaseAddress = "https://auth.sphere.io/";
            HttpClient client = new HttpClient();
            // normally client would be created by IHttpClientFactory
            IAuthorizationClient authorizationClient = new AuthorizationClient(client);
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(authorizationClient, clientConfiguration);
            Token token = tokenProvider.Token;
            Assert.NotNull(token.AccessToken);
        }

        [Fact]
        public void GetClientCredentialsTokenWithScope()
        {
            IClientConfiguration clientConfiguration = new ClientConfiguration();
            clientConfiguration.ClientId = "jBRiUPK2i9BuavWFsNZtyZt2";
            clientConfiguration.ClientSecret = "";
            clientConfiguration.Scope = "view_products:portablevendor";
            clientConfiguration.AuthorizationBaseAddress = "https://auth.sphere.io/";
            HttpClient client = new HttpClient();
            IAuthorizationClient authorizationClient = new AuthorizationClient(client);
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(authorizationClient, clientConfiguration);
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
    }
}