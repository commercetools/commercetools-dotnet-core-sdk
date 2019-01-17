using System;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class CreateTokenForEmailVerificationRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public CreateTokenForEmailVerificationRequestMessageBuilder(
            IClientConfiguration clientConfiguration,
            IEndpointRetriever endpointRetriever,
            ISerializerService serializerService)
            : base(clientConfiguration, endpointRetriever, null)
        {
            this.serializerService = serializerService;
        }

        private static HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(CreateTokenForEmailVerificationCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), this.GetHttpContent(command), HttpMethod);
        }

        private HttpContent GetHttpContent<T>(CreateTokenForEmailVerificationCommand<T> command)
        {
            var requestBody = new
            {
                Id = command.Id,
                Version = command.Version,
                TtlMinutes = command.TimeToLiveMinutes
            };
            return new StringContent(this.serializerService.Serialize(requestBody));
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/email-token";
            return new Uri(requestUri);
        }
    }
}
