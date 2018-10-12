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
        private readonly IExpansionVisitor expansionVisitor;

        public QueryRequestMessageBuilder(IClientConfiguration clientConfiguration, IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor, IExpansionVisitor expansionVisitor) : base(clientConfiguration)
        {
            this.queryPredicateExpressionVisitor = queryPredicateExpressionVisitor;
            this.expansionVisitor = expansionVisitor;
        }

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(QueryCommand<T> command)
        {            
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null);
        }

        private Uri GetRequestUri<T>(QueryCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            string where = queryPredicateExpressionVisitor.ProcessExpression(command.QueryPredicate.Expression);
            var parametersToAdd = new Dictionary<string, string>();
            parametersToAdd.Add("where", where);            
            foreach (var expansion in command.Expand)
            {
                string expand = this.expansionVisitor.GetPath(expansion.Expression);
                parametersToAdd.Add("expand", expand);
            }
            var newUri = QueryHelpers.AddQueryString(requestUri, parametersToAdd);
            return new Uri(newUri);
        }
    }
}