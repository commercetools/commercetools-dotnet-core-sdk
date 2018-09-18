using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using System;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateByKeyRequestMessageBuilder : RequestMessageBuilderBase
    {
        public override Type CommandType => typeof(UpdateByKeyCommand);
        private UpdateByKeyCommand command;
        private readonly ISerializerService serializerService;

        public UpdateByKeyRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
        }

        protected override HttpContent HttpContent
        {
            get
            {
                var requestBody = new
                {
                    Version = this.command.Version,
                    Actions = this.command.UpdateActions
                };
                return new StringContent(this.serializerService.Serialize(requestBody));
            }
        }

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            this.command = command as UpdateByKeyCommand;
            return this.GetRequestMessage<T>();
        }

        protected override Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + $"/key={this.command.Key}";
            return new Uri(requestUri);
        }
    }
}