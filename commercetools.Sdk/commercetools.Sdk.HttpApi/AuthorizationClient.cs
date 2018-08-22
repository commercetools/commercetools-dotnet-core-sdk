namespace commercetools.Sdk.HttpApi
{
    using System;
    using System.Net.Http;

    public class AuthorizationClient : IAuthorizationClient
    {
        private HttpClient client;
        
        public AuthorizationClient()
        {
            // TODO See if client has to be put back in the constructor to support AddHttpClient
            this.client = new HttpClient();
            // TODO Move to configuration
            client.BaseAddress = new Uri("https://auth.sphere.io/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public HttpClient Client
        {
            get
            {
                return this.client;
            }
        }
    }
}