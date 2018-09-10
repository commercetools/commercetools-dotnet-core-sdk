using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace commercetools.Sdk.HttpApi
{
    public abstract class TokenProvider
    {
        private ITokenStoreManager tokenStoreManager;

        protected IHttpClientFactory HttpClientFactory { get; }
        protected IClientConfiguration ClientConfiguration { get; }

        public TokenProvider(IHttpClientFactory httpClientFactory, IClientConfiguration clientConfiguration, ITokenStoreManager tokenStoreManager)
        {
            this.HttpClientFactory = httpClientFactory;
            this.ClientConfiguration = clientConfiguration;
            this.tokenStoreManager = tokenStoreManager;
        }

        public Token Token
        {
            get
            {
                Token token = this.tokenStoreManager.Token;
                if (token == null)
                {
                    token = GetTokenAsync(this.GetRequestMessage()).Result;
                    this.tokenStoreManager.Token = token;
                    return token;
                }
                if (token.Expired && !string.IsNullOrEmpty(token.RefreshToken))
                {
                    token = GetTokenAsync(this.GetRefreshTokenRequestMessage()).Result;
                    this.tokenStoreManager.Token = token;
                    return token;
                }
                return token;
            }
        }

        private async Task<Token> GetTokenAsync(HttpRequestMessage requestMessage)
        {
            HttpClient client = this.HttpClientFactory.CreateClient("auth");
            var result = await client.SendAsync(this.GetRequestMessage());
            string content = await result.Content.ReadAsStringAsync();
            // TODO ensure status 200
            return JsonConvert.DeserializeObject<Token>(content);
        }

        private HttpRequestMessage GetRefreshTokenRequestMessage()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string requestUri = this.ClientConfiguration.AuthorizationBaseAddress + $"oauth/token?grant_type=refresh_token";
            requestUri += $"&refresh_token={this.tokenStoreManager.Token.RefreshToken}";
            request.RequestUri = new Uri(requestUri);
            string credentials = $"{this.ClientConfiguration.ClientId}:{this.ClientConfiguration.ClientSecret}";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials)));
            request.Method = HttpMethod.Post;
            return request;
        }

        public abstract HttpRequestMessage GetRequestMessage();
    }
}
