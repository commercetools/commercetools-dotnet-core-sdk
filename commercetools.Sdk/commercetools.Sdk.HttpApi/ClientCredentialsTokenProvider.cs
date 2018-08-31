namespace commercetools.Sdk.HttpApi
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ClientCredentialsTokenProvider : ITokenProvider
    {
        private IHttpClientFactory httpClientFactory;
        private IClientConfiguration clientConfiguration;
        private ISessionManager sessionManager;
        public TokenFlow TokenFlow => TokenFlow.ClientCredentials;

        // TODO Maybe move to a parent class, it might be the same as in other providers
        public Token Token
        {
            get
            {
                Token token = this.sessionManager.Token;
                if (token == null || token.Expired)
                {
                    token = GetTokenTask().Result;
                    this.sessionManager.Token = token;
                }
                return token;
            }
        }

        public ClientCredentialsTokenProvider(IHttpClientFactory httpClientFactory, IClientConfiguration clientConfiguration, ISessionManager sessionManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.clientConfiguration = clientConfiguration;
            this.sessionManager = sessionManager;
        }

        private async Task<Token> GetTokenTask()
        {
            HttpClient client = this.httpClientFactory.CreateClient("auth");
            var result = await client.SendAsync(this.GetRequestMessage());
            string content = await result.Content.ReadAsStringAsync();
            // TODO ensure status 200
            return JsonConvert.DeserializeObject<Token>(content);
        }

        private HttpRequestMessage GetRequestMessage()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            // TODO Check if base address ends with trailing slash
            string requestUri = this.clientConfiguration.AuthorizationBaseAddress + "oauth/token?grant_type=client_credentials";
            if (!string.IsNullOrEmpty(this.clientConfiguration.Scope))
            {
                requestUri += $"&scope={this.clientConfiguration.Scope}";
            }
            request.RequestUri = new Uri(requestUri);
            string credentials = $"{this.clientConfiguration.ClientId}:{this.clientConfiguration.ClientSecret}";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials)));
            request.Method = HttpMethod.Post;
            return request;
        }
    }
}