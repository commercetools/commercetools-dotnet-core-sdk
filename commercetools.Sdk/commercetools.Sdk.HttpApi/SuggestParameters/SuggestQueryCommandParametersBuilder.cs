using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Suggestions;
using commercetools.Sdk.HttpApi.SearchParameters;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.SuggestParameters
{
    public class SuggestQueryCommandParametersBuilder : IQueryParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(SuggestQueryCommandParameters);
        }

        public List<KeyValuePair<string, string>> GetQueryParameters(IQueryParameters queryParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            var queryCommandParameters = queryParameters as SuggestQueryCommandParameters;
            if (queryCommandParameters == null)
            {
                return parameters;
            }

            parameters.AddRange(AddSearchKeywordsLanguageParameter(queryCommandParameters));

            var fuzzy = queryCommandParameters.Fuzzy ? "true" : "false";
            var staged = queryCommandParameters.Staged ? "true" : "false";

            parameters.Add(new KeyValuePair<string, string>("fuzzy", fuzzy));
            parameters.Add(new KeyValuePair<string, string>("staged", staged));

            if (queryCommandParameters.Limit != null)
            {
                parameters.Add(new KeyValuePair<string, string>("limit", queryCommandParameters.Limit.ToString()));
            }

            return parameters;
        }

        private static List<KeyValuePair<string, string>> AddSearchKeywordsLanguageParameter(SuggestQueryCommandParameters suggestQueryCommandParameters)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (suggestQueryCommandParameters.SearchKeywords != null)
            {
                foreach (var language in suggestQueryCommandParameters.SearchKeywords.Keys)
                {
                    queryStringParameters.Add(new KeyValuePair<string, string>($"searchKeywords.{language}", suggestQueryCommandParameters.SearchKeywords[language]));
                }
            }

            return queryStringParameters;
        }
    }
}
