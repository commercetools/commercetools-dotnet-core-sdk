namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.LinqToQueryPredicate;
    using commercetools.Sdk.Serialization;
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    public class QueryRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor;
        private readonly IExpansionExpressionVisitor expansionExpressionVisitor;
        private readonly ISortExpressionVisitor sortExpressionVisitor;

        public QueryRequestMessageBuilder(IClientConfiguration clientConfiguration, IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor, IExpansionExpressionVisitor expansionExpressionVisitor, ISortExpressionVisitor sortExpressionVisitor) : base(clientConfiguration)
        {
            this.queryPredicateExpressionVisitor = queryPredicateExpressionVisitor;
            this.expansionExpressionVisitor = expansionExpressionVisitor;
            this.sortExpressionVisitor = sortExpressionVisitor;
        }

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(QueryCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null);
        }

        private Uri GetRequestUri<T>(QueryCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            var queryStringParameters = new Dictionary<string, string>();
            AddQueryPredicateParameter(queryStringParameters, command);
            AddExpandParameters(queryStringParameters, command);
            AddSortParameters(queryStringParameters, command);
            var newUri = QueryHelpers.AddQueryString(requestUri, queryStringParameters);
            return new Uri(newUri);
        }

        private void AddQueryPredicateParameter<T>(Dictionary<string, string> queryStringParameters, QueryCommand<T> command)
        {
            if (command.QueryPredicate != null)
            {
                string where = queryPredicateExpressionVisitor.ProcessExpression(command.QueryPredicate.Expression);
                queryStringParameters.Add("where", where);
            }
        }

        private void AddExpandParameters<T>(Dictionary<string, string> queryStringParameters, QueryCommand<T> command)
        {
            if (command.Expand != null)
            {
                foreach (var expansion in command.Expand)
                {
                    string expandPath = this.expansionExpressionVisitor.GetPath(expansion.Expression);
                    queryStringParameters.Add("expand", expandPath);
                }
            }
        }

        private void AddSortParameters<T>(Dictionary<string, string> queryStringParameters, QueryCommand<T> command)
        {            
            if (command.Sort != null)
            {
                foreach (var sort in command.Sort)
                {
                    string sortPath = this.sortExpressionVisitor.GetPath(sort.Expression);
                    if (sort.SortDirection != null)
                    {
                        if (sort.SortDirection == SortDirection.Descending)
                        {
                            sortPath += " desc";
                        }
                        else
                        {
                            sortPath += " asc";
                        }
                    }
                    queryStringParameters.Add("sort", sortPath);
                }
            }
        }
    }
}