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
        public QueryRequestMessageBuilder(
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory)
            : base(endpointRetriever, parametersBuilderFactory)
        {
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
            if (command.Where != null)
            {
                queryStringParameters.AddRange(command.Where.Select(x => new KeyValuePair<string, string>("where", x)));
            }

            if (command.Expand != null)
            {
                queryStringParameters.AddRange(command.Expand.Select(x => new KeyValuePair<string, string>("expand", x)));
            }

            if (command.Sort != null)
            {
                queryStringParameters.AddRange(command.Sort.Select(x => new KeyValuePair<string, string>("sort", x)));
            }

            if (command.Limit != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("limit", command.Limit.ToString()));
            }

            if (command.Offset != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("offset", command.Offset.ToString()));
            }

            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri, UriKind.Relative);
        }
    }
}
