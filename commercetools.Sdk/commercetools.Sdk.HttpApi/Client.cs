using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
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
                    this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(this.GetClientUserAgent());
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

            // it will not reach this because either it will success and return the deserialized object or fail and handled by ErrorHandler which will throw it using the right exception type
            var exception = new ApiException(result.ReasonPhrase);
            throw exception;
        }

        /// <summary>
        /// Get UserAgent which we will set in the http client
        /// </summary>
        /// <returns>user agent with assembly version and .Net version</returns>
        private string GetClientUserAgent()
        {
            string assemblyVersion = Assembly
                .GetExecutingAssembly().GetName().Version.ToString();
            //ToDO: Make dotnet version string in better way, it now return (commercetools-dotnet-core-sdk/1.0.0.0 .NET Core 4.6.27317.03) and should return something like ( commercetools-dotnet-core-sdk/1.0.0.0 .NET-Core/4.6.27317.03)
            string dotNetVersion = RuntimeInformation.FrameworkDescription;
            string userAgent = $"commercetools-dotnet-core-sdk/{assemblyVersion}";
            return userAgent;
        }
    }
}
