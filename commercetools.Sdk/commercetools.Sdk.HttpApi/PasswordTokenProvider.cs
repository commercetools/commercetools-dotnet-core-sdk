namespace commercetools.Sdk.HttpApi
{
    using System;
    using System.Net.Http;

    // TODO Check how to pass username and password without saving them in objects
    public class PasswordTokenProvider : TokenProvider, ITokenProvider
    {
        public TokenFlow TokenFlow => TokenFlow.Password;
        private IUserCredentialsStoreManager userCredentialsManager;

        public PasswordTokenProvider(IHttpClientFactory httpClientFactory, IClientConfiguration clientConfiguration, IUserCredentialsStoreManager userCredentialsStoreManager) : base(httpClientFactory, clientConfiguration, userCredentialsStoreManager)
        {
            this.userCredentialsManager = userCredentialsStoreManager;
        }

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