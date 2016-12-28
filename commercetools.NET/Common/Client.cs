using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<Response<T>> GetAsync<T>(string endpoint, NameValueCollection values = null)
        {
            Response<T> response = new Response<T>();

            EnsureToken();

            if (this.Token == null)
            {
                response.Success = false;
                response.Errors.Add(new ErrorMessage("no_token", "Could not retrieve token"));
                return response;
            }

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

                HttpResponseMessage httpResponseMessage = await client.GetAsync(url);

                response = await GetResponse<T>(httpResponseMessage);
            }

            return response;
        }

        /// <summary>
        /// Executes a POST request.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="payload">Body of the request</param>
        /// <returns>JSON object</returns>
        public async Task<Response<T>> PostAsync<T>(string endpoint, string payload)
        {
            Response<T> response = new Response<T>();

            EnsureToken();

            if (this.Token == null)
            {
                response.Success = false;
                response.Errors.Add(new ErrorMessage("no_token", "Could not retrieve token"));
                return response;
            }

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
                HttpResponseMessage httpResponseMessage = await client.PostAsync(url, content);

                response = await GetResponse<T>(httpResponseMessage);
            }

            return response;
        }

        /// <summary>
        /// Executes a DELETE request.
        /// </summary>
        /// <param name="endpoint">API endpoint, excluding the project key</param>
        /// <param name="values">Values</param>
        /// <returns>JSON object</returns>
        public async Task<Response<T>> DeleteAsync<T>(string endpoint, NameValueCollection values = null)
        {
            Response<T> response = new Response<T>();

            EnsureToken();

            if (this.Token == null)
            {
                response.Success = false;
                response.Errors.Add(new ErrorMessage("no_token", "Could not retrieve token"));
                return response;
            }

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

                HttpResponseMessage httpResponseMessage = await client.DeleteAsync(url);

                response = await GetResponse<T>(httpResponseMessage);
            }

            return response;
        }

        #endregion

        #region Token Methods

        /// <summary>
        /// Ensures that the token for this instance has been retrieved and that it has not expired.
        /// </summary>
        private void EnsureToken()
        {
            if (this.Token == null)
            {
                Task<Response<Token>> task = GetTokenAsync();
                task.Wait();
                Response<Token> response = task.Result;

                if (response.Success)
                {
                    this.Token = response.Result;
                }
            }
            /*
             * The refresh token flow is currently only available for the password flow, which is currently not supported by the SDK.
             * More info: https://dev.commercetools.com/http-api-authorization.html#password-flow
             * 
            else if (this.Token.IsExpired())
            {
                this.Token = RefreshTokenAsync(this.Token.RefreshToken);
            }
             */
        }

        /// <summary>
        /// Retrieves a token from the authorization API using the client credentials flow.
        /// </summary>
        /// <returns>Token</returns>
        /// <see href="http://dev.commercetools.com/http-api-authorization.html#authorization-flows"/>
        public async Task<Response<Token>> GetTokenAsync()
        {
            Response<Token> response = new Response<Token>();

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

                HttpResponseMessage httpResponseMessage = await client.PostAsync(this.Configuration.OAuthUrl, content);
                response = await GetResponse<Token>(httpResponseMessage);
            }

            return response;
        }

        /// <summary>
        /// Refreshes a token from the authorization API using the refresh token flow.
        /// </summary>
        /// <param name="refreshToken">Refresh token value from the current token</param>
        /// <returns>Token</returns>
        /// <see href="http://dev.commercetools.com/http-api-authorization.html#authorization-flows"/>
        public async Task<Response<Token>> RefreshTokenAsync(string refreshToken)
        {
            Response<Token> response = new Response<Token>();

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

                HttpResponseMessage httpResponseMessage = await client.PostAsync(this.Configuration.OAuthUrl, content);
                response = await GetResponse<Token>(httpResponseMessage);
            }

            return response;
        }

        #endregion

        #region Utitlity

        /// <summary>
        /// Gets a response object from the API response.
        /// </summary>
        /// <typeparam name="T">Type of result</typeparam>
        /// <param name="httpResponseMessage">HttpResponseMessage</param>
        /// <returns>Response</returns>
        private async Task<Response<T>> GetResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            Response<T> response = new Response<T>();
            Type resultType = typeof(T);

            response.StatusCode = (int)httpResponseMessage.StatusCode;

            if (response.StatusCode >= 200 && response.StatusCode < 300)
            {
                response.Success = true;

                if (resultType == typeof(JObject))
                {
                    response.Result = JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                }
                else
                {
                    dynamic data = JsonConvert.DeserializeObject(await httpResponseMessage.Content.ReadAsStringAsync());
                    ConstructorInfo constructor = Helper.GetConstructorWithDataParameter(resultType);

                    if (constructor != null)
                    {
                        Helper.ObjectActivator<T> activator = Helper.GetActivator<T>(constructor);
                        response.Result = activator(data);
                    }
                }
            }
            else
            {
                JObject data = JsonConvert.DeserializeObject<JObject>(await httpResponseMessage.Content.ReadAsStringAsync());

                response.Success = false;
                response.Errors = new List<ErrorMessage>();

                if (data != null && (data["errors"] != null))
                {
                    foreach (JObject error in data["errors"])
                    {
                        if (error.HasValues)
                        {
                            string code = error.Value<string>("code");
                            string message = error.Value<string>("message");
                            response.Errors.Add(new ErrorMessage(code, message));
                        }
                    }
                }
            }

            return response;
        }

        #endregion
    }
}
