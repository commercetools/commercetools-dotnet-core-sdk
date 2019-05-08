using System;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class CreateTokenForPasswordResetRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public CreateTokenForPasswordResetRequestMessageBuilder(
            IEndpointRetriever endpointRetriever,
            ISerializerService serializerService)
            : base(endpointRetriever, null)
        {
            this.serializerService = serializerService;
        }

        private static HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(CreateTokenForPasswordResetCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), this.GetHttpContent(command), HttpMethod);
        }

        private HttpContent GetHttpContent<T>(CreateTokenForPasswordResetCommand<T> command)
        {
            var requestBody = new
            {
                Email = command.Email,
                TtlMinutes = command.TimeToLiveMinutes
            };
            return new StringContent(this.serializerService.Serialize(requestBody));
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/password-token";
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
