using System.Collections.Generic;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.HttpApi.SearchParameters
{
    public interface IQueryParametersBuilder : IParametersBuilder
    {
        List<KeyValuePair<string, string>> GetQueryParameters(IQueryParameters queryParameters);
    }
}
