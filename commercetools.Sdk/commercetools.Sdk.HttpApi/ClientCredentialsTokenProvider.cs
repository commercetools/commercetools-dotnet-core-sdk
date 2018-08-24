namespace commercetools.Sdk.HttpApi
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ClientCredentialsTokenProvider : ITokenProvider
    {
        private IAuthorizationClient authorizationClient;
        private IClientConfiguration clientConfiguration;
        private Token token;
        public TokenFlow TokenFlow = TokenFlow.ClientCredentials;

        // TODO Maybe move to a parent class, it might be the same as in other providers
        public Token Token
        {
            get
            {
                if (token == null || token.Expired)
                {
                    this.token = GetTokenTask().Result;
                }                
                return this.token;
            }
        }

        public ClientCredentialsTokenProvider(IAuthorizationClient authorizationClient, IClientConfiguration clientConfiguration)
        {
            this.authorizationClient = authorizationClient;
            this.clientConfiguration = clientConfiguration;
        }

        private async Task<Token> GetTokenTask()
        {
            HttpClient client = this.authorizationClient.Client;
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