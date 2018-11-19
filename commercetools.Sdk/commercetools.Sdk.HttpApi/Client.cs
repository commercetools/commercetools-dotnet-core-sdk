namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.HttpApi.Domain;
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    // commerce tools client, do not mistake for HttpClient
    public class Client : IClient
    {
        private readonly HttpClient client;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ISerializerService serializerService;
        private readonly IHttpApiCommandFactory httpApiCommandFactory;

        public Client(IHttpClientFactory httpClientFactory, IHttpApiCommandFactory httpApiCommandFactory, ISerializerService serializerService)
        {
            this.httpClientFactory = httpClientFactory;
            this.client = this.httpClientFactory.CreateClient(this.Name);
            this.serializerService = serializerService;
            this.httpApiCommandFactory = httpApiCommandFactory;
        }

        public string Name { get; set; } = "api";

        public async Task<T> ExecuteAsync<T>(Command<T> command)
        {
            IHttpApiCommand httpApiCommand = this.httpApiCommandFactory.Create(command);
            return await SendRequest<T>(httpApiCommand.HttpRequestMessage);
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage requestMessage)
        {
            var result = await this.client.SendAsync(requestMessage);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            { 
                return this.serializerService.Deserialize<T>(content);
            }
            if (content != null)
            {
                HttpApiClientException httpApiClientException = this.serializerService.Deserialize<HttpApiClientException>(content);
                throw httpApiClientException;
            }
            // TODO see what to do in else case
            return default(T);
        }
    }
}