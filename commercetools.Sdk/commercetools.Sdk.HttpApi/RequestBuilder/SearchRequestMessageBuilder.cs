namespace commercetools.Sdk.HttpApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Linq;
    using Microsoft.AspNetCore.WebUtilities;

    public class SearchRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public SearchRequestMessageBuilder(IClientConfiguration clientConfiguration, 
            IEndpointRetriever endpointRetriever,
            IQueryStringRequestBuilderFactory queryStringRequestBuilderFactory) : base(clientConfiguration, endpointRetriever, queryStringRequestBuilderFactory)
        {
        }

        private HttpMethod HttpMethod => HttpMethod.Post;

        private HttpContent GetHttpContent<T>(SearchCommand<T> command)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.AddRange(this.AddTextLanguageParameter(command));
            parameters.AddRange(this.AddParameters(command.Filter, "filter"));
            parameters.AddRange(this.AddParameters(command.FilterQuery, "filter.query"));
            parameters.AddRange(this.AddParameters(command.FilterFacets, "filter.facets"));
            parameters.AddRange(this.AddParameters(command.Facets, "facet"));
            parameters.AddRange(this.AddParameters(command.Sort, "sort"));
            if (command.Fuzzy != null)
            { 
                parameters.Add(new KeyValuePair<string, string>("fuzzy", command.Fuzzy.ToString()));
            }

            if (command.FuzzyLevel != null)
            {
                parameters.Add(new KeyValuePair<string, string>("fuzzyLevel", command.FuzzyLevel.ToString()));
            }

            if (command.Limit != null)
            {
                parameters.Add(new KeyValuePair<string, string>("limit", command.Limit.ToString()));
            }

            if (command.Offset != null)
            {
                parameters.Add(new KeyValuePair<string, string>("offset", command.Offset.ToString()));
            }

            if (command.MarkMatchingVariants != null)
            {
                parameters.Add(new KeyValuePair<string, string>("markMatchingVariants", command.MarkMatchingVariants.ToString()));
            }

            parameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            return new FormUrlEncodedContent(parameters);
        }

        public HttpRequestMessage GetRequestMessage<T>(SearchCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), this.GetHttpContent(command), this.HttpMethod);
        }

        private Uri GetRequestUri<T>(SearchCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + "/search";
            return new Uri(requestUri);
        }

        private List<KeyValuePair<string, string>> AddTextLanguageParameter<T>(SearchCommand<T> command)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command?.Text != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>($"text.{command.Text.Language}", command.Text.Term));
            }

            return queryStringParameters;
        }

        private List<KeyValuePair<string, string>> AddParameters(List<string> parameters, string parameterName)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (parameters == null)
            {
                return queryStringParameters;
            }

            foreach (var filter in parameters)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>(parameterName, filter));
            }

            return queryStringParameters;
        }
    }
}