using System.Collections.Generic;
using System.Globalization;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.SearchParameters
{
    public class ProductProjectionSearchParametersBuilder : ISearchParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(ProductProjectionSearchParameters);
        }

        public List<KeyValuePair<string, string>> GetSearchParameters<T>(ISearchParameters<T> searchParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            ProductProjectionSearchParameters productProjectionSearchParameters = searchParameters as ProductProjectionSearchParameters;
            if (productProjectionSearchParameters == null)
            {
                return parameters;
            }

            parameters.AddRange(AddTextLanguageParameter(productProjectionSearchParameters));
            parameters.AddRange(AddParameters(productProjectionSearchParameters.Filter, "filter"));
            parameters.AddRange(AddParameters(productProjectionSearchParameters.FilterQuery, "filter.query"));
            parameters.AddRange(AddParameters(productProjectionSearchParameters.FilterFacets, "filter.facets"));
            parameters.AddRange(AddParameters(productProjectionSearchParameters.Facets, "facet"));
            parameters.AddRange(AddParameters(productProjectionSearchParameters.Sort, "sort"));
            parameters.AddRange(AddParameters(productProjectionSearchParameters.Expand, "expand"));
            if (productProjectionSearchParameters.Fuzzy != null)
            {
                parameters.Add(new KeyValuePair<string, string>("fuzzy", productProjectionSearchParameters.Fuzzy.ToString()));
            }

            if (productProjectionSearchParameters.FuzzyLevel != null)
            {
                parameters.Add(new KeyValuePair<string, string>("fuzzyLevel", productProjectionSearchParameters.FuzzyLevel.ToString()));
            }

            if (productProjectionSearchParameters.Limit != null)
            {
                parameters.Add(new KeyValuePair<string, string>("limit", productProjectionSearchParameters.Limit.ToString()));
            }

            if (productProjectionSearchParameters.Offset != null)
            {
                parameters.Add(new KeyValuePair<string, string>("offset", productProjectionSearchParameters.Offset.ToString()));
            }

            if (productProjectionSearchParameters.MarkMatchingVariants != null)
            {
                parameters.Add(new KeyValuePair<string, string>("markMatchingVariants", productProjectionSearchParameters.MarkMatchingVariants.ToString()));
            }

            if (productProjectionSearchParameters.PriceCurrency != null)
            {
                parameters.Add(new KeyValuePair<string, string>("priceCurrency", productProjectionSearchParameters.PriceCurrency));
            }

            if (productProjectionSearchParameters.PriceCountry != null)
            {
                parameters.Add(new KeyValuePair<string, string>("priceCountry", productProjectionSearchParameters.PriceCountry));
            }

            if (productProjectionSearchParameters.PriceCustomerGroup != null)
            {
                parameters.Add(new KeyValuePair<string, string>("priceCustomerGroup", productProjectionSearchParameters.PriceCustomerGroup));
            }

            if (productProjectionSearchParameters.PriceChannel != null)
            {
                parameters.Add(new KeyValuePair<string, string>("priceChannel", productProjectionSearchParameters.PriceChannel));
            }

            var withTotal = productProjectionSearchParameters.WithTotal ? "true" : "false";
            parameters.Add(new KeyValuePair<string, string>("withTotal", withTotal));

            return parameters;
        }

        private static List<KeyValuePair<string, string>> AddTextLanguageParameter(ProductProjectionSearchParameters productProjectionSearchParameters)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (productProjectionSearchParameters?.Text != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>($"text.{productProjectionSearchParameters.Text.Language}", productProjectionSearchParameters.Text.Term));
            }

            return queryStringParameters;
        }

        private static List<KeyValuePair<string, string>> AddParameters(List<string> parameters, string parameterName)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            foreach (var filter in parameters)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>(parameterName, filter));
            }

            return queryStringParameters;
        }
    }
}
