using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShippingMethods;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class GetShippingMethodsForLocationAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(GetShippingMethodsForLocationAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters<T>(IAdditionalParameters<T> additionalParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            GetShippingMethodsForLocationAdditionalParameters getShippingMethodAdditionalParameters = additionalParameters as GetShippingMethodsForLocationAdditionalParameters;
            if (getShippingMethodAdditionalParameters == null)
            {
                return parameters;
            }

            if (getShippingMethodAdditionalParameters.Country != null)
            {
                parameters.Add(new KeyValuePair<string, string>("country", getShippingMethodAdditionalParameters.Country));
            }

            if (getShippingMethodAdditionalParameters.Currency != null)
            {
                parameters.Add(new KeyValuePair<string, string>("currency", getShippingMethodAdditionalParameters.Currency));
            }

            if (getShippingMethodAdditionalParameters.State != null)
            {
                parameters.Add(new KeyValuePair<string, string>("state", getShippingMethodAdditionalParameters.State));
            }

            return parameters;
        }
    }
}
