namespace commercetools.Sdk.HttpApi.Tokens
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Domain;
    using Serialization;

    internal abstract class TokenProvider
    {
        private readonly ISerializerService serializerService;
        private readonly ITokenStoreManager tokenStoreManager;

        protected TokenProvider(IHttpClientFactory httpClientFactory, IClientConfiguration clientConfiguration, ITokenStoreManager tokenStoreManager, ISerializerService serializerService)
        {
            this.HttpClientFactory = httpClientFactory;
            this.ClientConfiguration = clientConfiguration;
            this.tokenStoreManager = tokenStoreManager;
            this.serializerService = serializerService;
        }

        public Token Token
        {
            get
            {
                Token token = this.tokenStoreManager.Token;
                if (token == null)
                {
                    token = this.GetTokenAsync(this.GetRequestMessage()).Result;
                    this.tokenStoreManager.Token = token;
                    return token;
                }

                if (token.Expired && !string.IsNullOrEmpty(token.RefreshToken))
                {
                    token = this.GetTokenAsync(this.GetRefreshTokenRequestMessage()).Result;
                    this.tokenStoreManager.Token = token;
                    return token;
                }

                return token;
            }
        }

        protected IClientConfiguration ClientConfiguration { get; }

        protected IHttpClientFactory HttpClientFactory { get; }

        public abstract HttpRequestMessage GetRequestMessage();

        private HttpRequestMessage GetRefreshTokenRequestMessage()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string requestUri = this.ClientConfiguration.AuthorizationBaseAddress + "oauth/token?grant_type=refresh_token";
            requestUri += $"&refresh_token={this.tokenStoreManager.Token.RefreshToken}";
            request.RequestUri = new Uri(requestUri);
            string credentials = $"{this.ClientConfiguration.ClientId}:{this.ClientConfiguration.ClientSecret}";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials)));
            request.Method = HttpMethod.Post;
            return request;
        }

        private async Task<Token> GetTokenAsync(HttpRequestMessage requestMessage)
        {
            HttpClient client = this.HttpClientFactory.CreateClient("auth");
            var result = await client.SendAsync(requestMessage).ConfigureAwait(false);
            string content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                return this.serializerService.Deserialize<Token>(content);
            }

            HttpApiClientException generalClientException = new HttpApiClientException
            {
                StatusCode = (int)result.StatusCode,
                Message = result.ReasonPhrase
            };
            throw generalClientException;
        }
    }
}