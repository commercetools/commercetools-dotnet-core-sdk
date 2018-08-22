namespace commercetools.Sdk.HttpApi
{
    using System;
    using System.Net.Http;

    public class ClientCredentialsTokenProvider : ITokenProvider
    {
        private IAuthorizationClient authorizationClient;
        private IClientConfiguration clientConfiguration; 

        public Token GetToken()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string credentials = $"{this.clientConfiguration.ClientId}:{this.clientConfiguration.ClientSecret}";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials)));

            return new Token();
        }

        public ClientCredentialsTokenProvider(IAuthorizationClient authorizationClient, IClientConfiguration clientConfiguration)
        {
            this.authorizationClient = authorizationClient;
            this.clientConfiguration = clientConfiguration;
        }
    }
}
