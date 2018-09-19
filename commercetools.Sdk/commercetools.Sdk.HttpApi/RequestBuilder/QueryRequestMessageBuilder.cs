namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net.Http;

    public class QueryRequestMessageBuilder : RequestMessageBuilderBase
    {
        private readonly ISerializerService serializerService;

        public QueryRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
        }

        private HttpContent GetHttpContent<T>(QueryCommand<T> command)
        {
            return null;
            //return new StringContent(this.serializerService.Serialize(command));
        }

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            Type typeOfCommand = command.GetType();

            if (!(command is QueryCommand<T> queryCommand))
            {
                throw new InvalidCastException();
            }
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), this.GetHttpContent<T>(queryCommand));
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/";
            return new Uri(requestUri);
        }
    }
}