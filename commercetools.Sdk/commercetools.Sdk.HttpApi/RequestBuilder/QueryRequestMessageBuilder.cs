namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
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
            queryStringParameters.AddRange(AddQueryPredicateParameter(command));
            if (command.Expand != null)
            {
                queryStringParameters.AddRange(command.Expand.GetQueryStringParameters(this.expansionExpressionVisitor));
            }
            queryStringParameters.AddRange(AddSortParameters(command));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri);
        }

        private List<KeyValuePair<string, string>> AddQueryPredicateParameter<T>(QueryCommand<T> command)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.QueryPredicate != null)
            {
                string where = queryPredicateExpressionVisitor.ProcessExpression(command.QueryPredicate.Expression);
                queryStringParameters.Add(new KeyValuePair<string, string>("where", where));
            }
            return queryStringParameters;
        }

        private List<KeyValuePair<string, string>> AddSortParameters<T>(QueryCommand<T> command)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.Sort != null)
            {
                foreach (var sort in command.Sort)
                {
                    string sortPath = this.sortExpressionVisitor.Render(sort.Expression);
                    sortPath += GetSortDirectionPath(sort.SortDirection);
                    queryStringParameters.Add(new KeyValuePair<string, string>("sort", sortPath));
                }
            }
            return queryStringParameters;
        }

        private string GetSortDirectionPath(SortDirection? sortDirection)
        {
            string sortPath = string.Empty;
            if (sortDirection != null)
            {
                if (sortDirection == SortDirection.Descending)
                {
                    sortPath = " desc";
                }
                else
                {
                    sortPath = " asc";
                }
            }
            return sortPath;
        }
    }
}