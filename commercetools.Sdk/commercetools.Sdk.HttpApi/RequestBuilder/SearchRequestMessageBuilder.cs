namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.LinqToQueryPredicate;
    using System;
    using System.Net.Http;

    public class SearchRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly IFilterExpressionVisitor filterExpressionVisitor;
        private readonly IFacetExpressionVisitor facetExpressionVisitor;

        public SearchRequestMessageBuilder(IClientConfiguration clientConfiguration, IFilterExpressionVisitor filterExpressionVisitor, IFacetExpressionVisitor facetExpressionVisitor) : base(clientConfiguration)
        {
            this.filterExpressionVisitor = filterExpressionVisitor;
            this.facetExpressionVisitor = facetExpressionVisitor;
        }

        private HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(SearchCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), null, this.HttpMethod);
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/";
            return new Uri(requestUri);
        }
    }
}