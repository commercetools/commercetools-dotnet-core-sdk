using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;

namespace commercetools.Sdk.HttpApi
{
    public class CtpClientFactory : ICtpClientFactory
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IHttpApiCommandFactory httpApiCommandFactory;
        private readonly ISerializerService serializerService;

        public CtpClientFactory(IHttpClientFactory clientFactory, IHttpApiCommandFactory httpApiCommandFactory, ISerializerService serializerService)
        {
            this.clientFactory = clientFactory;
            this.httpApiCommandFactory = httpApiCommandFactory;
            this.serializerService = serializerService;
        }

        public IClient Create(string name)
        {
            return new CtpClient(clientFactory, httpApiCommandFactory, serializerService) { Name = name };
        }
    }
}
