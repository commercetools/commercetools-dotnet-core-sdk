using System;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class ChangePasswordRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public ChangePasswordRequestMessageBuilder(
            IClientConfiguration clientConfiguration,
            IEndpointRetriever endpointRetriever,
            ISerializerService serializerService)
            : base(clientConfiguration, endpointRetriever, null)
        {
            this.serializerService = serializerService;
        }

        private HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(ChangePasswordCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri(command), this.GetHttpContent(command), this.HttpMethod);
        }

        private HttpContent GetHttpContent<T>(ChangePasswordCommand<T> command)
        {
            var requestBody = new
            {
                Version = command.Version,
                Id = command.Id,
                CurrentPassword = command.CurrentPassword,
                NewPassword = command.NewPassword
            };
            return new StringContent(this.serializerService.Serialize(requestBody));
        }

        private Uri GetRequestUri<T>(ChangePasswordCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + "/password";
            return new Uri(requestUri);
        }
    }
}