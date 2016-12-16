namespace commercetools.Common
{
    /// <summary>
    /// A set of configuration variables needed for making requests with the client.
    /// </summary>
    public class Configuration
    {
        #region Properties

        public string OAuthUrl { get; private set; }
        public string ApiUrl { get; private set; }
        public string ProjectKey { get; private set;}
        public string ClientID { get; private set; }
        public string ClientSecret { get; private set; }
        public ProjectScope Scope { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oAuthUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="projectKey"></param>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        /// <param name="scope"></param>
        public Configuration(string oAuthUrl, string apiUrl, string projectKey, string clientID, string clientSecret, ProjectScope scope)
        {
            this.OAuthUrl = oAuthUrl;
            this.ApiUrl = apiUrl;
            this.ProjectKey = projectKey;
            this.ClientID = clientID;
            this.ClientSecret = clientSecret;
            this.Scope = scope;

            if (this.OAuthUrl.EndsWith("/"))
            {
                this.OAuthUrl = this.OAuthUrl.Remove(this.OAuthUrl.Length - 1);
            }

            if (this.ApiUrl.EndsWith("/"))
            {
                this.ApiUrl = apiUrl.Remove(this.ApiUrl.Length - 1);
            }
        }

        #endregion
    }
}
