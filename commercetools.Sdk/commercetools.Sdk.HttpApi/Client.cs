namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    // commerce tools client, do not mistake for HttpClient
    public class Client : IClient
    {
        public string Name { get; set; }
        private IHttpClientFactory httpClientFactory;
        private HttpClient client;
        private IRequestBuilder requestBuilder;
        private ISerializerService serializerService;

        public async Task<T> GetByIdAsync<T>(Guid guid)
        {
            return await SendRequest<T>(this.requestBuilder.GetRequestMessageById<T>(guid));
        }

        public async Task<T> GetByKeyAsync<T>(string key)
        {
            return await SendRequest<T>(this.requestBuilder.GetRequestMessageByKey<T>(key));
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage requestMessage)
        {
            var result = await this.client.SendAsync(requestMessage);
            string content = await result.Content.ReadAsStringAsync();
            return this.serializerService.Deserialize<T>(content);
        }

        public async Task<T> Execute<T>(ICommand command)
        {
            // TODO See how to better cast between interfaces and do some checkss
            return await SendRequest<T>(this.requestBuilder.GetRequestMessage<T>((IHttpApiCommand)command));
        }

        public Client(IHttpClientFactory httpClientFactory, IRequestBuilder requestBuilder, ISerializerService serializerService)
        {            
            this.httpClientFactory = httpClientFactory;
            this.client = this.httpClientFactory.CreateClient("api");
            this.requestBuilder = requestBuilder;
            this.serializerService = serializerService;
        }
    }
}