namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.LinqToQueryPredicate;
    using commercetools.Sdk.Serialization;
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Net;
    using System.Net.Http;

    public class QueryRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly ISerializerService serializerService;
        private readonly IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor;

        public QueryRequestMessageBuilder(ISerializerService serializerService, IClientConfiguration clientConfiguration, IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor) : base(clientConfiguration)
        {
            this.serializerService = serializerService;
            this.queryPredicateExpressionVisitor = queryPredicateExpressionVisitor;
        }

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(QueryCommand<T> command)
        {            
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null);
        }

        private Uri GetRequestUri<T>(QueryCommand<T> command)
        {
            string where = queryPredicateExpressionVisitor.ProcessExpression(command.QueryPredicate.Expression);
            var parametersToAdd = new System.Collections.Generic.Dictionary<string, string> { { "where", where } };
            string requestUri = this.GetMessageBase<T>();
            var newUri = QueryHelpers.AddQueryString(requestUri, parametersToAdd);
            return new Uri(newUri);
        }
    }
}