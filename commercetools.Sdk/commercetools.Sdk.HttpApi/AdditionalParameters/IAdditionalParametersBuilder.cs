using System.Collections.Generic;
using commercetools.Sdk.Domain;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public interface IAdditionalParametersBuilder : IParametersBuilder
    {
        List<KeyValuePair<string, string>> GetAdditionalParameters<T>(IAdditionalParameters<T> additionalParameters);
    }
}
