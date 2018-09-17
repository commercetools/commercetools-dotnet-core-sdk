namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Serialization;
    using System.Net.Http;
    using System.Threading.Tasks;

    // commerce tools client, do not mistake for HttpClient
    public class Client : IClient
    {
        private readonly HttpClient client;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRequestBuilder requestBuilder;
        private readonly ISerializerService serializerService;

        public Client(IHttpClientFactory httpClientFactory, IRequestBuilder requestBuilder, ISerializerService serializerService)
        {
            this.httpClientFactory = httpClientFactory;
            this.client = this.httpClientFactory.CreateClient(this.Name);
            this.requestBuilder = requestBuilder;
            this.serializerService = serializerService;
        }

        public string Name { get; set; } = "api";

        public async Task<T> Execute<T>(ICommand command)
        {
            return await SendRequest<T>(this.requestBuilder.GetRequestMessage<T>(command));
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage requestMessage)
        {
            var result = await this.client.SendAsync(requestMessage);
            string content = await result.Content.ReadAsStringAsync();
            return this.serializerService.Deserialize<T>(content);
        }
    }
}