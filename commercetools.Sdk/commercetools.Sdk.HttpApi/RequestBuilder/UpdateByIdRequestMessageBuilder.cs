using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using System;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateByIdRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public Type CommandType => typeof(UpdateByIdCommand);
        private UpdateByIdCommand command;
        private readonly ISerializerService serializerService;

        public UpdateByIdRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
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

        public HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            this.command = command as UpdateByIdCommand;
            return this.GetRequestMessage<T>();
        }

        protected override Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + $"/{this.command.Guid}";
            return new Uri(requestUri);
        }
    }
}