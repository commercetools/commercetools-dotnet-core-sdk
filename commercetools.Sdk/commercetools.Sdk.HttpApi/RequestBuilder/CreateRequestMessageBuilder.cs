namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net.Http;

    public class CreateRequestMessageBuilder : RequestMessageBuilderBase
    {
        private readonly ISerializerService serializerService;

        public CreateRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
        }

        private HttpContent GetHttpContent<T>(CreateCommand<T> command)
        {
            return new StringContent(this.serializerService.Serialize(command.Entity));
        }

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            if (!(command is CreateCommand<T> createCommand))
            {
                throw new InvalidCastException();
            }
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), this.GetHttpContent<T>(createCommand));
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/";
            return new Uri(requestUri);
        }
    }
}