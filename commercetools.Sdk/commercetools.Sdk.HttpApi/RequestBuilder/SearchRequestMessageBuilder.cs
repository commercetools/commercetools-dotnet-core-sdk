namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Domain;
    using commercetools.Sdk.Linq;
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    // TODO See if there is a need to split this class due to the huge number of search command properties
    public class SearchRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly IFilterExpressionVisitor filterExpressionVisitor;

        public SearchRequestMessageBuilder(IClientConfiguration clientConfiguration, IFilterExpressionVisitor filterExpressionVisitor) : base(clientConfiguration)
        {
            this.filterExpressionVisitor = filterExpressionVisitor;
        }

        private HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(SearchCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null, this.HttpMethod);
        }

        private Uri GetRequestUri<T>(SearchCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + "/search";
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            queryStringParameters.AddRange(AddTextLanguageParameter(command));
            queryStringParameters.AddRange(AddFilterParameter(command.Filter, "filter"));
            queryStringParameters.AddRange(AddFilterParameter(command.FilterQuery, "filter.query"));
            queryStringParameters.AddRange(AddFilterParameter(command.FilterFacets, "filter.facets"));
            queryStringParameters.AddRange(AddFacetParameter(command));
            string newUri = requestUri;
            queryStringParameters.ForEach(x => { newUri = QueryHelpers.AddQueryString(newUri, x.Key, x.Value); });
            return new Uri(newUri);
        }

        private List<KeyValuePair<string, string>> AddTextLanguageParameter<T>(SearchCommand<T> command)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.Text != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>($"text.{command.Text.Language}", command.Text.Term));
            }
            return queryStringParameters;
        }

        private List<KeyValuePair<string, string>> AddFilterParameter<T>(List<Filter<T>> filters, string parameterName)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    string filterPath = this.filterExpressionVisitor.Render(filter.Expression);
                    queryStringParameters.Add(new KeyValuePair<string, string>(parameterName, filterPath));
                }
            }
            return queryStringParameters;
        }

        private List<KeyValuePair<string, string>> AddFacetParameter<T>(SearchCommand<T> command)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.Facets != null)
            {
                foreach (var facet in command.Facets)
                {
                    string facetPath = GetFacetPath(facet);
                    if (facet.Alias != null)
                    {
                        facetPath += $" as {facet.Alias}";
                    }
                    if (facet.IsCountingProducts == true)
                    {
                        facetPath += $" counting products";
                    }
                    queryStringParameters.Add(new KeyValuePair<string, string>("facet", facetPath));
                }
            }
            return queryStringParameters;
        }

        private string GetFacetPath<T>(Facet<T> facet)
        {
            return this.filterExpressionVisitor.Render(facet.Expression);
        }
    }
}