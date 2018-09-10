namespace commercetools.Sdk.HttpApi
{
    using Newtonsoft.Json;
    using System;

    public class Token
    {
        public Token()
        {
            this.CreationDate = DateTime.Now;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime CreationDate { get; private set; }

        public bool Expired
        {
            get
            {
                // TODO Implement based on creation time
                return false;
            }
        }
    }
}