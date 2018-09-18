using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using System;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateByIdRequestMessageBuilder : RequestMessageBuilderBase
    {
        private readonly ISerializerService serializerService;

        public UpdateByIdRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
        }

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            if (!(command is UpdateByIdCommand<T> updateByIdCommand))
            {
                throw new InvalidCastException();
            }
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(updateByIdCommand), this.GetHttpContent<T>(updateByIdCommand));
        }

        private HttpContent GetHttpContent<T>(UpdateByIdCommand<T> command)
        {
            var requestBody = new
            {
                Version = command.Version,
                Actions = command.UpdateActions
            };
            return new StringContent(this.serializerService.Serialize(requestBody));
            
        }
        private Uri GetRequestUri<T>(UpdateByIdCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/{command.Guid}";
            return new Uri(requestUri);
        }
    }
}