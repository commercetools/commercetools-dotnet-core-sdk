using System.Collections.Generic;
using commercetools.Sdk.Domain;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class ProductAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(ProductAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters(IAdditionalParameters additionalParameters)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            ProductAdditionalParameters productAdditionalParameters = additionalParameters as ProductAdditionalParameters;
            if (productAdditionalParameters == null)
            {
                return queryStringParameters;
            }

            if (productAdditionalParameters.PriceChannel != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("priceChannel", productAdditionalParameters.PriceChannel));
            }

            if (productAdditionalParameters.PriceCountry != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("priceCountry", productAdditionalParameters.PriceCountry));
            }

            if (productAdditionalParameters.PriceCurrency != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("priceCurrency", productAdditionalParameters.PriceCurrency));
            }

            if (productAdditionalParameters.PriceCustomerGroup != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("priceCustomerGroup", productAdditionalParameters.PriceCustomerGroup));
            }

            return queryStringParameters;
        }
    }
}
