namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net.Http;

    public class ClientCredentialsTokenProvider : TokenProvider, ITokenProvider
    {
        public ClientCredentialsTokenProvider(IHttpClientFactory httpClientFactory, IClientConfiguration clientConfiguration, ITokenStoreManager tokenStoreManager, ISerializerService serializerService) : base(httpClientFactory, clientConfiguration, tokenStoreManager, serializerService)
        {
        }

        public TokenFlow TokenFlow => TokenFlow.ClientCredentials;
        public override HttpRequestMessage GetRequestMessage()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            // TODO Check if base address ends with trailing slash
            string requestUri = this.ClientConfiguration.AuthorizationBaseAddress + "oauth/token?grant_type=client_credentials";
            if (!string.IsNullOrEmpty(this.ClientConfiguration.Scope))
            {
                requestUri += $"&scope={this.ClientConfiguration.Scope}";
            }
            request.RequestUri = new Uri(requestUri);
            string credentials = $"{this.ClientConfiguration.ClientId}:{this.ClientConfiguration.ClientSecret}";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials)));
            request.Method = HttpMethod.Post;
            return request;
        }
    }
}