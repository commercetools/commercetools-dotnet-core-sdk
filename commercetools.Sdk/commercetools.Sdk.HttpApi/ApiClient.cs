namespace commercetools.Sdk.HttpApi
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using commercetools.Sdk.Domain;
    using Newtonsoft.Json;

    public class ApiClient : IApiClient
    {
        private ITokenProvider tokenProvider;

        public ApiClient(HttpClient client, ITokenProvider tokenProvider)
        {
            this.Client = client;
            this.tokenProvider = tokenProvider;
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public HttpClient Client { get; }

        // TODO Maybe it will be switched to async only, which means this is not needed
        public Category GetCategoryById(int categoryId)
        {
            return GetCategoryByIdTask(categoryId).Result;
        }

        private async Task<Category> GetCategoryByIdTask(int categoryId)
        {
            var result = await this.Client.SendAsync(this.GetRequestMessage());
            string content = await result.Content.ReadAsStringAsync();
            // TODO Do not use Newtonsoft here directly, but move it to another project
            return JsonConvert.DeserializeObject<Category>(content);
        }

        private HttpRequestMessage GetRequestMessage()
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            // TODO Set all necessary headers, body, uri etc.
            // TODO Ensure authorization
            EnsureAuthorization(requestMessage);
            return requestMessage;
        }

        // TODO This is just a way to implement this, probably needs to be moved from this class
        private void EnsureAuthorization(HttpRequestMessage requestMessage)
        {
            Token token = this.tokenProvider.Token;
            // TODO Set token to request message
        }
    }
}