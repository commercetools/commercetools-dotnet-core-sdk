namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.HttpApi.RequestBuilders;
    using commercetools.Sdk.Linq;
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public class QueryRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor;
        private readonly IExpansionExpressionVisitor expansionExpressionVisitor;
        private readonly ISortExpressionVisitor sortExpressionVisitor;

        public QueryRequestMessageBuilder(IClientConfiguration clientConfiguration, IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor, IExpansionExpressionVisitor expansionExpressionVisitor, ISortExpressionVisitor sortExpressionVisitor, IEndpointRetriever endpointRetriever) : base(clientConfiguration, endpointRetriever)
        {
            this.queryPredicateExpressionVisitor = queryPredicateExpressionVisitor;
            this.expansionExpressionVisitor = expansionExpressionVisitor;
            this.sortExpressionVisitor = sortExpressionVisitor;
        }

        private HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(QueryCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null, this.HttpMethod);
        }

        private Uri GetRequestUri<T>(QueryCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.QueryPredicate != null)
            {
                queryStringParameters.AddRange(command.QueryPredicate.GetQueryStringParameters(queryPredicateExpressionVisitor));
            }
                
            if (command.Expand != null)
            {
                queryStringParameters.AddRange(command.Expand.GetQueryStringParameters(this.expansionExpressionVisitor));
            }

            if (command.Sort != null)
            {
                queryStringParameters.AddRange(command.Sort.GetQueryStringParameters(this.sortExpressionVisitor));
            }

            if (command.Limit != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("limit", command.Limit.ToString()));
            }

            if (command.Offset != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("offset", command.Offset.ToString()));
            }

            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });            
            return new Uri(requestUri);
        }
    }
}