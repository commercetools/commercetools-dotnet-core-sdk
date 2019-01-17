using System;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class ResetPasswordRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public ResetPasswordRequestMessageBuilder(
            IClientConfiguration clientConfiguration,
            IEndpointRetriever endpointRetriever,
            ISerializerService serializerService)
            : base(clientConfiguration, endpointRetriever, null)
        {
            this.serializerService = serializerService;
        }

        private static HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(ResetPasswordCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), this.GetHttpContent(command), HttpMethod);
        }

        private HttpContent GetHttpContent<T>(ResetPasswordCommand<T> command)
        {
            var requestBody = new
            {
                Version = command.Version,
                TokenValue = command.TokenValue,
                NewPassword = command.NewPassword
            };
            return new StringContent(this.serializerService.Serialize(requestBody));
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/password/reset";
            return new Uri(requestUri);
        }
    }
}