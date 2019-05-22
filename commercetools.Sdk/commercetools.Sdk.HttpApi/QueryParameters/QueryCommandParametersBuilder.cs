using System.Collections.Generic;
using System.Globalization;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.SearchParameters
{
    public class QueryCommandParametersBuilder : IQueryParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(QueryCommandParameters);
        }

        public List<KeyValuePair<string, string>> GetQueryParameters(IQueryParameters queryParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            QueryCommandParameters queryCommandParameters = queryParameters as QueryCommandParameters;
            if (queryCommandParameters == null)
            {
                return parameters;
            }

            parameters.AddRange(AddParameters(queryCommandParameters.Where, "where"));
            parameters.AddRange(AddParameters(queryCommandParameters.Sort, "sort"));
            parameters.AddRange(AddParameters(queryCommandParameters.Expand, "expand"));

            if (queryCommandParameters.Limit != null)
            {
                parameters.Add(new KeyValuePair<string, string>("limit", queryCommandParameters.Limit.ToString()));
            }

            if (queryCommandParameters.Offset != null)
            {
                parameters.Add(new KeyValuePair<string, string>("offset", queryCommandParameters.Offset.ToString()));
            }

            var withTotal = queryCommandParameters.WithTotal ? "true" : "false";
            parameters.Add(new KeyValuePair<string, string>("withTotal", withTotal));

            return parameters;
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
