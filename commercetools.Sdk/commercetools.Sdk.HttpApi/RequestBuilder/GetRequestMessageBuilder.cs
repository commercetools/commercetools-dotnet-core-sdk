namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class GetRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public GetRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(GetCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null);
        }

        private Uri GetRequestUri<T>(GetCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            if (command.ParameterKey == "id")
            {
                requestUri += $"/{command.ParameterValue}";
            }
            else
            {
                requestUri += $"/{command.ParameterKey}={command.ParameterValue}";
            }
            
            return new Uri(requestUri);
        }
    }
}