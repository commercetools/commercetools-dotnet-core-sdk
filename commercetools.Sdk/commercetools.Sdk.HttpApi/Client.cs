using System.Net.Http;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Serialization;

namespace commercetools.Sdk.HttpApi
{
    public class Client : IClient
    {
        private readonly IHttpApiCommandFactory httpApiCommandFactory;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ISerializerService serializerService;
        private HttpClient httpClient;

        public Client(IHttpClientFactory httpClientFactory, IHttpApiCommandFactory httpApiCommandFactory, ISerializerService serializerService)
        {
            this.httpClientFactory = httpClientFactory;
            this.serializerService = serializerService;
            this.httpApiCommandFactory = httpApiCommandFactory;
        }

        public string Name { get; set; } = DefaultClientNames.Api;

        private HttpClient HttpClient
        {
            get
            {
                if (this.httpClient == null)
                {
                    this.httpClient = this.httpClientFactory.CreateClient(this.Name);
                }

                return this.httpClient;
            }
        }

        public async Task<T> ExecuteAsync<T>(Command<T> command)
        {
            IHttpApiCommand httpApiCommand = this.httpApiCommandFactory.Create(command);
            return await this.SendRequest<T>(httpApiCommand.HttpRequestMessage).ConfigureAwait(false);
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage requestMessage)
        {
            var result = await this.HttpClient.SendAsync(requestMessage).ConfigureAwait(false);
            string content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                return this.serializerService.Deserialize<T>(content);
            }

            if (!string.IsNullOrEmpty(content))
            {
                HttpApiClientException httpApiClientException = this.serializerService.Deserialize<HttpApiClientException>(content);
                throw httpApiClientException;
            }

            HttpApiClientException generalClientException = new HttpApiClientException
            {
                StatusCode = (int)result.StatusCode,
                Message = result.ReasonPhrase
            };
            throw generalClientException;
        }
    }
}