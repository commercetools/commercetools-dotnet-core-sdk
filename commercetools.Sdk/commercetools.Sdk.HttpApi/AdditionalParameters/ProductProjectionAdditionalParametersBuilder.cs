using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductProjections;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.AdditionalParameters
{
    public class ProductProjectionAdditionalParametersBuilder : IAdditionalParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(ProductProjectionAdditionalParameters);
        }

        public List<KeyValuePair<string, string>> GetAdditionalParameters(IAdditionalParameters additionalParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            ProductProjectionAdditionalParameters productAdditionalParameters = additionalParameters as ProductProjectionAdditionalParameters;
            if (productAdditionalParameters == null)
            {
                return parameters;
            }

            if (productAdditionalParameters.PriceChannel != null)
            {
                parameters.Add(new KeyValuePair<string, string>("priceChannel", productAdditionalParameters.PriceChannel));
            }

            if (productAdditionalParameters.PriceCountry != null)
            {
                parameters.Add(new KeyValuePair<string, string>("priceCountry", productAdditionalParameters.PriceCountry));
            }

            if (productAdditionalParameters.PriceCurrency != null)
            {
                parameters.Add(new KeyValuePair<string, string>("priceCurrency", productAdditionalParameters.PriceCurrency));
            }

            if (productAdditionalParameters.PriceCustomerGroup != null)
            {
                parameters.Add(new KeyValuePair<string, string>("priceCustomerGroup", productAdditionalParameters.PriceCustomerGroup));
            }

            if (productAdditionalParameters.Staged != null)
            {
                parameters.Add(new KeyValuePair<string, string>("staged", productAdditionalParameters.Staged.ToString()));
            }

            if (productAdditionalParameters.StoreProjection != null)
            {
                parameters.Add(new KeyValuePair<string, string>("storeProjection", productAdditionalParameters.StoreProjection));
            }

            if (productAdditionalParameters.LocaleProjection != null)
            {
                parameters.Add(new KeyValuePair<string, string>("localeProjection", productAdditionalParameters.LocaleProjection));
            }

            return parameters;
        }
    }
}
