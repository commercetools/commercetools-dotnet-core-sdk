namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net.Http;

    public class CreateRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public Type CommandType => typeof(CreateCommand);

        private CreateCommand command;
        private ISerializerService serializerService;

        public CreateRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
        }

        protected override HttpContent HttpContent
        {
            get
            {
                return new StringContent(this.serializerService.Serialize(this.command.Entity));
            }
        }

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            this.command = command as CreateCommand;
            return this.GetRequestMessage<T>();
        }

        protected override Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/";
            return new Uri(requestUri);
        }
    }
}