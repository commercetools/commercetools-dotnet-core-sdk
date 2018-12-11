using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShippingMethods;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class ShippingMethodsForOrderEditAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(ShippingMethodsForOrderEditAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters<T>(IAdditionalParameters<T> additionalParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            ShippingMethodsForOrderEditAdditionalParameters shippingMethodAdditionalParameters = additionalParameters as ShippingMethodsForOrderEditAdditionalParameters;
            if (shippingMethodAdditionalParameters == null)
            {
                return parameters;
            }

            if (shippingMethodAdditionalParameters.Country != null)
            {
                parameters.Add(new KeyValuePair<string, string>("country", shippingMethodAdditionalParameters.Country));
            }

            if (shippingMethodAdditionalParameters.OrderEditId != null)
            {
                parameters.Add(new KeyValuePair<string, string>("orderEditId", shippingMethodAdditionalParameters.OrderEditId));
            }

            if (shippingMethodAdditionalParameters.State != null)
            {
                parameters.Add(new KeyValuePair<string, string>("state", shippingMethodAdditionalParameters.State));
            }

            return parameters;
        }
    }
}
