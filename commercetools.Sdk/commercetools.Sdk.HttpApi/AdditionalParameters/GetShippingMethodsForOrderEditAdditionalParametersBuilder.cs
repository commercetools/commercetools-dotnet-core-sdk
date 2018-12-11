using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShippingMethods;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class GetShippingMethodsForOrderEditAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(GetShippingMethodsForOrderEditAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters<T>(IAdditionalParameters<T> additionalParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            GetShippingMethodsForOrderEditAdditionalParameters getShippingMethodAdditionalParameters = additionalParameters as GetShippingMethodsForOrderEditAdditionalParameters;
            if (getShippingMethodAdditionalParameters == null)
            {
                return parameters;
            }

            if (getShippingMethodAdditionalParameters.Country != null)
            {
                parameters.Add(new KeyValuePair<string, string>("country", getShippingMethodAdditionalParameters.Country));
            }

            if (getShippingMethodAdditionalParameters.OrderEditId != null)
            {
                parameters.Add(new KeyValuePair<string, string>("orderEditId", getShippingMethodAdditionalParameters.OrderEditId));
            }

            if (getShippingMethodAdditionalParameters.State != null)
            {
                parameters.Add(new KeyValuePair<string, string>("state", getShippingMethodAdditionalParameters.State));
            }

            return parameters;
        }
    }
}
