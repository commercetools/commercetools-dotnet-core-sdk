namespace XUnitTestProject1
{
    using commercetools.Sdk.HttpApi;
    using System;
    using System.Net.Http;
    using Xunit;

    public class Examples
    {
        [Fact]
        public void GetClientCredentialsToken()
        {
            IClientConfiguration clientConfiguration = new ClientConfiguration();
            clientConfiguration.ClientId = "pV7ogtY4wPRWbqQUQJ5TBWmh";
            clientConfiguration.ClientSecret = "s-mSeiIojSUUgiht5kAA_7cLvaxXrMl6";
            clientConfiguration.Scope = "manage_project:portablevendor";
            IAuthorizationClient authorizationClient = new AuthorizationClient();
            ITokenProvider tokenProvider = new ClientCredentialsTokenProvider(authorizationClient, clientConfiguration);
            Token token = tokenProvider.GetToken();
        }
    }
}
