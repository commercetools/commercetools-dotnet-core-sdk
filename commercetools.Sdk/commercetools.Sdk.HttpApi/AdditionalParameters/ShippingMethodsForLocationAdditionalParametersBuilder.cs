using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShippingMethods;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class ShippingMethodsForLocationAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(ShippingMethodsForLocationAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters<T>(IAdditionalParameters<T> additionalParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            ShippingMethodsForLocationAdditionalParameters shippingMethodAdditionalParameters = additionalParameters as ShippingMethodsForLocationAdditionalParameters;
            if (shippingMethodAdditionalParameters == null)
            {
                return parameters;
            }

            if (shippingMethodAdditionalParameters.Country != null)
            {
                parameters.Add(new KeyValuePair<string, string>("country", shippingMethodAdditionalParameters.Country));
            }

            if (shippingMethodAdditionalParameters.Currency != null)
            {
                parameters.Add(new KeyValuePair<string, string>("currency", shippingMethodAdditionalParameters.Currency));
            }

            if (shippingMethodAdditionalParameters.State != null)
            {
                parameters.Add(new KeyValuePair<string, string>("state", shippingMethodAdditionalParameters.State));
            }

            return parameters;
        }
    }
}
