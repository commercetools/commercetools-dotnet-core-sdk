using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShippingMethods;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class GetShippingMethodsForCartAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(GetShippingMethodsForCartAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters<T>(IAdditionalParameters<T> additionalParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            GetShippingMethodsForCartAdditionalParameters getShippingMethodAdditionalParameters = additionalParameters as GetShippingMethodsForCartAdditionalParameters;
            if (getShippingMethodAdditionalParameters == null)
            {
                return parameters;
            }

            if (getShippingMethodAdditionalParameters.CartId != null)
            {
                parameters.Add(new KeyValuePair<string, string>("cartId", getShippingMethodAdditionalParameters.CartId));
            }

            return parameters;
        }
    }
}
