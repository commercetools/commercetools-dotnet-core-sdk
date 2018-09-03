namespace commercetools.Sdk.HttpApi
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PasswordTokenProvider : ITokenProvider
    {
        private IHttpClientFactory httpClientFactory;
        private IClientConfiguration clientConfiguration;
        public TokenFlow TokenFlow => TokenFlow.Password;
        private ISessionManager sessionManager;

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

        public PasswordTokenProvider(IHttpClientFactory httpClientFactory, IClientConfiguration clientConfiguration, ISessionManager sessionManager)
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
            string requestUri = this.clientConfiguration.AuthorizationBaseAddress + $"oauth/{this.clientConfiguration.ProjectKey}/customers/token?grant_type=password";
            requestUri += $"&username={this.sessionManager.Username}";
            requestUri += $"&password={this.sessionManager.Password}";
            requestUri += $"&scope={this.clientConfiguration.Scope}";
            request.RequestUri = new Uri(requestUri);
            string credentials = $"{this.clientConfiguration.ClientId}:{this.clientConfiguration.ClientSecret}";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials)));
            request.Method = HttpMethod.Post;
            return request;
        }
    }
}