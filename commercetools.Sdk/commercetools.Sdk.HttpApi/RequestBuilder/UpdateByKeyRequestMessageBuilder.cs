using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using System;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateByKeyRequestMessageBuilder : RequestMessageBuilderBase
    {
        private readonly ISerializerService serializerService;

        public UpdateByKeyRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
        }

        private HttpContent GetHttpContent<T>(UpdateByKeyCommand<T> command)
        {           
            var requestBody = new
            {
                Version = command.Version,
                Actions = command.UpdateActions
            };
            return new StringContent(this.serializerService.Serialize(requestBody));

        }

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            if (!(command is UpdateByKeyCommand<T> updateByKeyCommand))
            {
                throw new InvalidCastException();
            }
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(updateByKeyCommand), this.GetHttpContent<T>(updateByKeyCommand));
        }

        private Uri GetRequestUri<T>(UpdateByKeyCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/key={command.Key}";
            return new Uri(requestUri);
        }
    }
}