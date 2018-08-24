namespace commercetools.Sdk.HttpApi
{
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class AnonymousSessionsTokenProvider : ITokenProvider
    {
        private IAuthorizationClient authorizationClient;
        private IClientConfiguration clientConfiguration;
        private Token token;

        public TokenFlow TokenFlow = TokenFlow.AnonymousSession;

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

        public AnonymousSessionsTokenProvider(IAuthorizationClient authorizationClient, IClientConfiguration clientConfiguration)
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
            // TODO Implement
            return request;
        }
    }
}