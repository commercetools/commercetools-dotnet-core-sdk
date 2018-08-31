namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Threading.Tasks;

    // commerce tools client, do not mistake for HttpClient
    public class Client : IClient
    {
        public string Name { get; set; }
        private IHttpClientFactory httpClientFactory;
        private HttpClient client;

        public Category GetCategoryById(int categoryId)
        {
            return GetCategoryByIdTask(categoryId).Result;
        }

        private async Task<Category> GetCategoryByIdTask(int categoryId)
        {
            var result = await this.client.SendAsync(this.GetRequestMessage());
            string content = await result.Content.ReadAsStringAsync();
            // TODO Do not use Newtonsoft here directly, but move it to another project
            return JsonConvert.DeserializeObject<Category>(content);
        }

        private HttpRequestMessage GetRequestMessage()
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            // TODO Set all necessary headers, body, uri etc.
            return requestMessage;
        }

        public Client(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.client = this.httpClientFactory.CreateClient("api");
        }
    }
}