using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.AdditionalParameters;
using commercetools.Sdk.HttpApi.SearchParameters;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class SearchRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        private readonly IParametersBuilderFactory<ISearchParametersBuilder> searchParametersBuilderFactory;

        public SearchRequestMessageBuilder(
            IClientConfiguration clientConfiguration,
            IEndpointRetriever endpointRetriever,
            IParametersBuilderFactory<IAdditionalParametersBuilder> parametersBuilderFactory,
            IParametersBuilderFactory<ISearchParametersBuilder> searchParametersBuilderFactory)
            : base(clientConfiguration, endpointRetriever, parametersBuilderFactory)
        {
            this.searchParametersBuilderFactory = searchParametersBuilderFactory;
        }

        private static HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(SearchCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), this.GetHttpContent(command), HttpMethod);
        }

        private HttpContent GetHttpContent<T>(SearchCommand<T> command)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            if (command.SearchParameters != null)
            {
                ISearchParametersBuilder searchParametersBuilder = this.searchParametersBuilderFactory.GetParameterBuilder(command.SearchParameters);
                parameters.AddRange(searchParametersBuilder.GetSearchParameters(command.SearchParameters));
            }

            if (command.Expand != null)
            {
                parameters.AddRange(command.Expand.Select(x => new KeyValuePair<string, string>("expand", x)));
            }

            parameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            return new FormUrlEncodedContent(parameters);
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/search";
            return new Uri(requestUri);
        }
    }
}
