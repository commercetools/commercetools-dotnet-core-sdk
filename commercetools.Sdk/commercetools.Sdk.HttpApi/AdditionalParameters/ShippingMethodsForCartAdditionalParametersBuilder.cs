using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ShippingMethods;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class ShippingMethodsForCartAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(ShippingMethodsForCartAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters<T>(IAdditionalParameters<T> additionalParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            ShippingMethodsForCartAdditionalParameters shippingMethodAdditionalParameters = additionalParameters as ShippingMethodsForCartAdditionalParameters;
            if (shippingMethodAdditionalParameters == null)
            {
                return parameters;
            }

            if (shippingMethodAdditionalParameters.CartId != null)
            {
                parameters.Add(new KeyValuePair<string, string>("cartId", shippingMethodAdditionalParameters.CartId));
            }

            return parameters;
        }
    }
}
