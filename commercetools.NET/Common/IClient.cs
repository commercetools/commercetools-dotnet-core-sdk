using System.Collections.Specialized;
using System.Threading.Tasks;

namespace commercetools.Common
{
    public interface IClient
    {
        /// <summary>
        /// Configuration
        /// </summary>
        Configuration Configuration { get; }

        /// <summary>
        /// Token
        /// </summary>
        Token Token { get; }

        /// <summary>
        /// The identifiying user agent that is included in all API requests.
        /// </summary>
        string UserAgent { get; }

        /// <summary>
        /// Executes a GET request.
        /// </summary>
        /// <param name="endpoint">API endpoint, excluding the project key</param>
        /// <param name="values">Values</param>
        /// <returns>JSON object</returns>
        Task<Response<T>> GetAsync<T>(string endpoint, NameValueCollection values = null);

        /// <summary>
        /// Executes a POST request.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="payload">Body of the request</param>
        /// <returns>JSON object</returns>
        Task<Response<T>> PostAsync<T>(string endpoint, string payload);

        /// <summary>
        /// Executes a DELETE request.
        /// </summary>
        /// <param name="endpoint">API endpoint, excluding the project key</param>
        /// <param name="values">Values</param>
        /// <returns>JSON object</returns>
        Task<Response<T>> DeleteAsync<T>(string endpoint, NameValueCollection values = null);

        /// <summary>
        /// Retrieves a token from the authorization API using the client credentials flow.
        /// </summary>
        /// <returns>Token</returns>
        /// <see href="http://dev.commercetools.com/http-api-authorization.html#authorization-flows"/>
        Task<Response<Token>> GetTokenAsync();

        /// <summary>
        /// Refreshes a token from the authorization API using the refresh token flow.
        /// </summary>
        /// <param name="refreshToken">Refresh token value from the current token</param>
        /// <returns>Token</returns>
        /// <see href="http://dev.commercetools.com/http-api-authorization.html#authorization-flows"/>
        Task<Response<Token>> RefreshTokenAsync(string refreshToken);
    }
}