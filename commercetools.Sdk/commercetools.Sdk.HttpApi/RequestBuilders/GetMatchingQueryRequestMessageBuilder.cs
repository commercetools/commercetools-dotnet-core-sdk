using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.ShippingMethods;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class GetMatchingQueryRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;

        public GetMatchingQueryRequestMessageBuilder(
            ISerializerService serializerService,
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(endpointRetriever, parametersBuilderFactory)
        {
            this.serializerService = serializerService;
        }

        private static HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(GetMatchingQueryCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null, HttpMethod);
        }

        private Uri GetRequestUri<T>(GetMatchingQueryCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + command.UrlSuffix;

            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.Expand != null)
            {
                queryStringParameters.AddRange(command.Expand.Select(x => new KeyValuePair<string, string>("expand", x)));
            }

            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
