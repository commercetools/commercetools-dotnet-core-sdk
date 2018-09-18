namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.Serialization;
    using System;
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

        //public async Task<T> CreateAsync<T>(IDraft<T> draft)
        //{
        //    CreateCommand createCommand = new CreateCommand(draft);
        //    return await SendRequest<T>(this.requestBuilder.GetRequestMessage<T>(createCommand));
        //}

        public async Task<T> Execute<T>(ICommand<T> command)
        {
            return await SendRequest<T>(this.requestBuilder.GetRequestMessage<T>(command));
        }

        //public async Task<T> Get<T>(Guid id)
        //{
        //    GetByIdCommand createCommand = new GetByIdCommand(id);
        //    return await SendRequest<T>(this.requestBuilder.GetRequestMessage<T>(createCommand));
        //}

        public Task<PagedQueryResult<T>> Query<T>()
        {
            throw new System.NotImplementedException();
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage requestMessage)
        {
            var result = await this.client.SendAsync(requestMessage);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            { 
                return this.serializerService.Deserialize<T>(content);
            }
            if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ResourceNotFoundException();
            }
            // TODO see what to do in else case
            return default(T);
        }
    }
}