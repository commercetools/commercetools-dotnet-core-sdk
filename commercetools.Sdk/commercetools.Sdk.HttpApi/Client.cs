namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.HttpApi.Domain;
    using commercetools.Sdk.Serialization;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Client : IClient
    {
        private readonly HttpClient client;
        private readonly ISerializerService serializerService;
        private readonly IHttpApiCommandFactory httpApiCommandFactory;

        public Client(IHttpClientFactory httpClientFactory, IHttpApiCommandFactory httpApiCommandFactory, ISerializerService serializerService)
        {
            this.client = httpClientFactory.CreateClient(this.Name);
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
            if (!string.IsNullOrEmpty(content))
            {
                HttpApiClientException httpApiClientException = this.serializerService.Deserialize<HttpApiClientException>(content);
                throw httpApiClientException;
            }
            HttpApiClientException generalClientException = new HttpApiClientException();
            generalClientException.StatusCode = (int)result.StatusCode;
            generalClientException.Message = result.ReasonPhrase;
            throw generalClientException;
        }
    }
}