using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// A client for executing requests against the commercetools web service.
    /// </summary>
    public class Client
    {
        #region Properties

        /// <summary>
        /// Configuration
        /// </summary>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// Token
        /// </summary>
        public Token Token { get; private set; }

        /// <summary>
        /// The identifiying user agent that is included in all API requests.
        /// </summary>
        public string UserAgent { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Client(Configuration configuration)
        {
            this.Configuration = configuration;

            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyVersion = assembly.GetName().Version.ToString();
            string dotNetVersion = Environment.Version.ToString();
            this.UserAgent = string.Format("commercetools-dotnet-sdk/{0} .NET/{1}", assemblyVersion, dotNetVersion);
        }

        #endregion

        #region Web Service Methods

        /// <summary>
        /// Executes a GET request.
        /// </summary>
        /// <param name="endpoint">API endpoint, excluding the project key</param>
        /// <param name="values">Values</param>
        /// <returns>JSON object</returns>
        public async Task<object> GetAsync(string endpoint, NameValueCollection values = null)
        {
            dynamic data = null;

            await EnsureToken();

            if (!string.IsNullOrWhiteSpace(endpoint) && !endpoint.StartsWith("/"))
            {
                endpoint = string.Concat("/", endpoint);
            }

            string url = string.Concat(this.Configuration.ApiUrl, "/", this.Configuration.ProjectKey, endpoint, values.ToQueryString());

            using (HttpClient client = new HttpClient(new ClientLoggingHandler(new HttpClientHandler())))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(this.Token.TokenType, this.Token.AccessToken);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(this.UserAgent);

                HttpResponseMessage response = await client.GetAsync(url);
                int statusCode = (int)response.StatusCode;
                data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                if (statusCode >= 400)
                {
                    throw new WebServiceHttpException(data);
                }
            }

            return data;
        }

        /// <summary>
        /// Executes a POST request.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="payload">Body of the request</param>
        /// <returns>JSON object</returns>
        public async Task<object> PostAsync(string endpoint, string payload)
        {
            dynamic data = null;

            await EnsureToken();

            if (!string.IsNullOrWhiteSpace(endpoint) && !endpoint.StartsWith("/"))
            {
                endpoint = string.Concat("/", endpoint);
            }

            string url = string.Concat(this.Configuration.ApiUrl, "/", this.Configuration.ProjectKey, endpoint);

            using (HttpClient client = new HttpClient(new ClientLoggingHandler(new HttpClientHandler())))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(this.Token.TokenType, this.Token.AccessToken);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(this.UserAgent);

                StringContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                int statusCode = (int)response.StatusCode;
                data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                if (statusCode >= 400)
                {
                    throw new WebServiceHttpException(data);
                }
            }

            return data;
        }

        /// <summary>
        /// Executes a DELETE request.
        /// </summary>
        /// <param name="endpoint">API endpoint, excluding the project key</param>
        /// <param name="values">Values</param>
        /// <returns>JSON object</returns>
        public async Task<object> DeleteAsync(string endpoint, NameValueCollection values = null)
        {
            dynamic data = null;

            await EnsureToken();

            if (!string.IsNullOrWhiteSpace(endpoint) && !endpoint.StartsWith("/"))
            {
                endpoint = string.Concat("/", endpoint);
            }

            string url = string.Concat(this.Configuration.ApiUrl, "/", this.Configuration.ProjectKey, endpoint, values.ToQueryString());

            using (HttpClient client = new HttpClient(new ClientLoggingHandler(new HttpClientHandler())))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(this.Token.TokenType, this.Token.AccessToken);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(this.UserAgent);

                HttpResponseMessage response = await client.DeleteAsync(url);
                int statusCode = (int)response.StatusCode;
                data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                if (statusCode >= 400)
                {
                    throw new WebServiceHttpException(data);
                }
            }

            return data;
        }

        #endregion

        #region Token Methods

        /// <summary>
        /// Ensures that the token for this instance has been retrieved and that it has not expired.
        /// </summary>
        private async Task EnsureToken()
        {
            if (this.Token == null)
            {
                this.Token = await GetTokenAsync();
            }
            /*
             * The refresh token flow is currently only available for the password flow, which is currently not supported by the SDK.
             * More info: https://dev.commercetools.com/http-api-authorization.html#password-flow
             * 
            else if (this.Token.IsExpired())
            {
                this.Token = await RefreshTokenAsync(this.Token.RefreshToken);
            }
             */
        }

        /// <summary>
        /// Retrieves a token from the authorization API using the client credentials flow.
        /// </summary>
        /// <returns>Token</returns>
        /// <see href="http://dev.commercetools.com/http-api-authorization.html#authorization-flows"/>
        public async Task<Token> GetTokenAsync()
        {
            Token token = null;

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", string.Concat(this.Configuration.Scope.ToEnumMemberString(), ":", this.Configuration.ProjectKey))
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Concat(this.Configuration.ClientID, ":", this.Configuration.ClientSecret)));

            using (HttpClient client = new HttpClient(new ClientLoggingHandler(new HttpClientHandler())))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(this.UserAgent);

                HttpResponseMessage response = await client.PostAsync(this.Configuration.OAuthUrl, content);
                int statusCode = (int)response.StatusCode;
                dynamic data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                if (statusCode >= 400)
                {
                    throw new WebServiceHttpException(data);
                }

                token = new Token(data);
            }

            return token;
        }

        /// <summary>
        /// Refreshes a token from the authorization API using the refresh token flow.
        /// </summary>
        /// <param name="refreshToken">Refresh token value from the current token</param>
        /// <returns>Token</returns>
        /// <see href="http://dev.commercetools.com/http-api-authorization.html#authorization-flows"/>
        public async Task<Token> RefreshTokenAsync(string refreshToken)
        {
            Token token = null;

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Concat(this.Configuration.ClientID, ":", this.Configuration.ClientSecret)));

            using (HttpClient client = new HttpClient(new ClientLoggingHandler(new HttpClientHandler())))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(this.UserAgent);

                HttpResponseMessage response = await client.PostAsync(this.Configuration.OAuthUrl, content);
                int statusCode = (int)response.StatusCode;
                dynamic data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                if (statusCode >= 400)
                {
                    throw new WebServiceHttpException(data);
                }

                token = new Token(data);
            }

            return token;
        }

        #endregion
    }
}