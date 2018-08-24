namespace commercetools.Sdk.HttpApi
{
    using System.Net.Http;

    public class AuthorizationClient : IAuthorizationClient
    {
        public AuthorizationClient(HttpClient client)
        {
            this.Client = client;
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public HttpClient Client { get; }
    }
}