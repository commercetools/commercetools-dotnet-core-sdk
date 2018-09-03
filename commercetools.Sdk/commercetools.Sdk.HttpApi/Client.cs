namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    // commerce tools client, do not mistake for HttpClient
    public class Client : IClient
    {
        public string Name { get; set; }
        private IHttpClientFactory httpClientFactory;
        private HttpClient client;
        private IClientConfiguration clientConfiguration;

        public Category GetCategoryById(Guid categoryId)
        {
            return GetCategoryByIdTask(categoryId).Result;
        }

        private async Task<Category> GetCategoryByIdTask(Guid categoryId)
        {
            var result = await this.client.SendAsync(this.GetRequestMessage(categoryId));
            string content = await result.Content.ReadAsStringAsync();
            // TODO Do not use Newtonsoft here directly, but move it to another project
            return JsonConvert.DeserializeObject<Category>(content);
        }

        private HttpRequestMessage GetRequestMessage(Guid id)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            string requestUri = this.clientConfiguration.ApiBaseAddress + $"{this.clientConfiguration.ProjectKey}/categories/{id}";
            request.RequestUri = new Uri(requestUri);
            request.Method = HttpMethod.Get;
            return request;
        }

        public Client(IHttpClientFactory httpClientFactory, IClientConfiguration clientConfiguration)
        {
            this.httpClientFactory = httpClientFactory;
            this.client = this.httpClientFactory.CreateClient("api");
            this.clientConfiguration = clientConfiguration;
        }
    }
}