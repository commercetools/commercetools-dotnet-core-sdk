using System;
using System.Collections.Generic;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class GetMatchingRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public GetMatchingRequestMessageBuilder(
            ISerializerService serializerService,
            IClientConfiguration clientConfiguration,
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(clientConfiguration, endpointRetriever, parametersBuilderFactory)
        {
            this.serializerService = serializerService;
        }

        private static HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(GetMatchingCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), this.GetHttpContent<T>(command), HttpMethod);
        }

        private HttpContent GetHttpContent<T>(GetMatchingCommand<T> command)
        {
            return new StringContent(this.serializerService.Serialize(command.Parameters));
        }

        private Uri GetRequestUri<T>(GetMatchingCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            requestUri += "/matching";
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri);
        }
    }
}