namespace commercetools.Sdk.HttpApi.Tokens
{
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net.Http;

    internal class PasswordTokenProvider : TokenProvider, ITokenProvider
    {
        private readonly IUserCredentialsStoreManager userCredentialsManager;

        public PasswordTokenProvider(IHttpClientFactory httpClientFactory, IClientConfiguration clientConfiguration, IUserCredentialsStoreManager userCredentialsStoreManager, ISerializerService serializerService) : base(httpClientFactory, clientConfiguration, userCredentialsStoreManager, serializerService)
        {
            this.userCredentialsManager = userCredentialsStoreManager;
        }

        public TokenFlow TokenFlow => TokenFlow.Password;

        public override HttpRequestMessage GetRequestMessage()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string requestUri = this.ClientConfiguration.AuthorizationBaseAddress + $"oauth/{this.ClientConfiguration.ProjectKey}/customers/token?grant_type=password";
            requestUri += $"&username={this.userCredentialsManager.Username}";
            requestUri += $"&password={this.userCredentialsManager.Password}";
            requestUri += $"&scope={this.ClientConfiguration.Scope}";
            request.RequestUri = new Uri(requestUri);
            string credentials = $"{this.ClientConfiguration.ClientId}:{this.ClientConfiguration.ClientSecret}";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials)));
            request.Method = HttpMethod.Post;
            return request;
        }
    }
}