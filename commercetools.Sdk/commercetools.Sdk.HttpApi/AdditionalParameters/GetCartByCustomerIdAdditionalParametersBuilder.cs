using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class GetCartByCustomerIdAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(GetCartByCustomerIdAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters(IAdditionalParameters additionalParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            GetCartByCustomerIdAdditionalParameters cartAdditionalParameters = additionalParameters as GetCartByCustomerIdAdditionalParameters;
            if (cartAdditionalParameters == null)
            {
                return parameters;
            }

            if (cartAdditionalParameters.CustomerId != null)
            {
                parameters.Add(new KeyValuePair<string, string>("customerId", cartAdditionalParameters.CustomerId));
            }

            return parameters;
        }
    }
}
