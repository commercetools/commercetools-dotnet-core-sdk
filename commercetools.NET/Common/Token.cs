using System;
using System.Linq;

using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// Represents a response from an authorization request using the client credentials flow.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-authorization.html#client-credentials-flow"/>
    public class Token
    {
        #region Properties

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; private set; }

        [JsonProperty(PropertyName = "expires_in")]
        public DateTime? ExpiryDate { get; private set; }

        [JsonProperty(PropertyName = "scope")]
        public ProjectScope? ProjectScope { get; private set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; private set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; private set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Token(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.AccessToken = data.access_token;
            int? expiresIn = data.expires_in;
            string scope = data.scope;
            this.RefreshToken = data.refresh_token;
            this.TokenType = data.token_type;

            if (expiresIn.HasValue)
            {
                // Offset slightly as per commercetools recommendation
                this.ExpiryDate = DateTime.UtcNow.AddSeconds(expiresIn.Value - 60);
            }

            if (!string.IsNullOrWhiteSpace(scope) && scope.Contains(':'))
            {
                string description = scope.Split(':')[0];

                ProjectScope? projectScope;
                this.ProjectScope = Helper.TryGetEnumByEnumMemberAttribute<ProjectScope?>(description, out projectScope) ? projectScope : null; 
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks the expiry date against the current time to see if the token has expired.
        /// </summary>
        public bool IsExpired()
        {
            if (this.ExpiryDate.HasValue)
            {
                return DateTime.UtcNow.CompareTo(this.ExpiryDate.Value) >= 0;
            }

            return true;
        }

        #endregion 
    }
}
