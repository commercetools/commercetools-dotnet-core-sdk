namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.Serialization;
    using System;
    using System.Net;
    using System.Net.Http;

    public class QueryRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public QueryRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
        }

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(QueryCommand<T> command)
        {            
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), null);
        }

        private Uri GetRequestUri<T>()
        {
            var parametersToAdd = new System.Collections.Generic.Dictionary<string, string> { { "where", "key = \"newKeyKMI\"" } };
            string requestUri = this.GetMessageBase<T>();
            var newUri = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(requestUri, parametersToAdd);
            return new Uri(newUri);
        }
    }
}