namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Serialization;
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public class CreateRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public CreateRequestMessageBuilder(ISerializerService serializerService, 
            IClientConfiguration clientConfiguration, 
            IEndpointRetriever endpointRetriever, 
            IQueryStringRequestBuilderFactory queryStringRequestBuilderFactory) : base(clientConfiguration, endpointRetriever, queryStringRequestBuilderFactory)
        {
            this.serializerService = serializerService;
        }

        private HttpContent GetHttpContent<T>(CreateCommand<T> command)
        {
            return new StringContent(this.serializerService.Serialize(command.Entity));
        }

        private HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(CreateCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), this.GetHttpContent<T>(command), this.HttpMethod);
        }

        private Uri GetRequestUri<T>(CreateCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + "/";
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            queryStringParameters.AddRange(this.GetAdditionalQueryStringParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri);
        }
    }
}