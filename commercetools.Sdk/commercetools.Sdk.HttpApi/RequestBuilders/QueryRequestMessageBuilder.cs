using commercetools.Sdk.HttpApi.SearchParameters;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using AdditionalParameters;
    using commercetools.Sdk.Client;
    using Microsoft.AspNetCore.WebUtilities;

    public class QueryRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly IParametersBuilderFactory<IQueryParametersBuilder> queryParametersBuilderFactory;

        public QueryRequestMessageBuilder(
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory,
            IParametersBuilderFactory<IQueryParametersBuilder> queryParametersBuilderFactory)
            : base(endpointRetriever, parametersBuilderFactory)
        {
            this.queryParametersBuilderFactory = queryParametersBuilderFactory;
        }

        private static HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(QueryCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null, HttpMethod);
        }

        private Uri GetRequestUri<T>(QueryCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();

            if (command.QueryParameters != null)
            {
                IQueryParametersBuilder queryParametersBuilder = this.queryParametersBuilderFactory.GetParameterBuilder(command.QueryParameters);
                queryStringParameters.AddRange(queryParametersBuilder.GetQueryParameters(command.QueryParameters));
            }

            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
