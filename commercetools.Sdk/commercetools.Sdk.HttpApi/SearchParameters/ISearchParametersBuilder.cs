using System.Collections.Generic;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.HttpApi.SearchParameters
{
    public interface ISearchParametersBuilder : IParametersBuilder
    {
        List<KeyValuePair<string, string>> GetSearchParameters<T>(ISearchParameters<T> searchParameters);
    }
}