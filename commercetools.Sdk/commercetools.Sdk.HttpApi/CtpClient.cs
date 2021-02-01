namespace commercetools.Sdk.HttpApi
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Client;
    using DelegatingHandlers;
    using Domain.Exceptions;
    using Serialization;

    public class CtpClient : IClient
    {
        private readonly IHttpApiCommandFactory httpApiCommandFactory;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ISerializerService serializerService;
        private readonly IUserAgentProvider userAgentProvider;
        private HttpClient httpClient;

        public CtpClient(
            IHttpClientFactory httpClientFactory,
            IHttpApiCommandFactory httpApiCommandFactory,
            ISerializerService serializerService,
            IUserAgentProvider userAgentProvider)
        {
            this.httpClientFactory = httpClientFactory;
            this.serializerService = serializerService;
            this.httpApiCommandFactory = httpApiCommandFactory;
            this.userAgentProvider = userAgentProvider;
        }

        public string Name { get; set; } = DefaultClientNames.Api;

        private HttpClient HttpClient => this.httpClient ?? (this.httpClient = this.httpClientFactory.CreateClient(this.Name));

        public async Task<T> ExecuteAsync<T>(ICommand<T> command)
        {
            var httpApiCommand = this.httpApiCommandFactory.Create(command);
            return await this.SendRequest<T>(httpApiCommand.HttpRequestMessage).ConfigureAwait(false);
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage requestMessage)
        {
            var result = await this.HttpClient.SendAsync(requestMessage).ConfigureAwait(false);
            var content = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                return this.serializerService.Deserialize<T>(content);
            }

            // it will not reach this because either it will success and return the deserialized object or fail and handled by ErrorHandler which will throw it using the right exception type
            var exception = new ApiException(result.ReasonPhrase);
            throw exception;
        }
    }
}