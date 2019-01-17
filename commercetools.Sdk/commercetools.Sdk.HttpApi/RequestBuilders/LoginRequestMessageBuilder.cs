using System;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class LoginRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public LoginRequestMessageBuilder(
            IClientConfiguration clientConfiguration,
            IEndpointRetriever endpointRetriever,
            ISerializerService serializerService)
            : base(clientConfiguration, endpointRetriever, null)
        {
            this.serializerService = serializerService;
        }

        private static HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(LoginCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri(), this.GetHttpContent(command), HttpMethod);
        }

        private HttpContent GetHttpContent<T>(LoginCommand<T> command)
        {
            var requestBody = new
            {
                Email = command.Email,
                Password = command.Password,
                AnonymousCartId = command.AnonymousCartId,
                AnonymousCartSignInMode = command.AnonymousCartSignInMode,
                AnonymousId = command.AnonymousId,
                UpdateProductData = command.UpdateProductData
            };
            return new StringContent(this.serializerService.Serialize(requestBody));
        }

        private Uri GetRequestUri()
        {
            string requestUri = this.GetMessageBaseWithoutEndpoint() + "/login";
            return new Uri(requestUri);
        }
    }
}
