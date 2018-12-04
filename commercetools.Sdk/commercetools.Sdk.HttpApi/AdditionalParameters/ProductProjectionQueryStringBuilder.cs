using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class ProductProjectionQueryStringBuilder : IQueryStringRequestBuilder<ProductProjection>
    {
        public List<KeyValuePair<string, string>> GetQueryStringParameters(IAdditionalParameters<ProductProjection> additionalParameters)
        {
            ProductProjectionAdditionalParameters productAdditionalParameters = additionalParameters as ProductProjectionAdditionalParameters;
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (productAdditionalParameters.PriceChannel != null)
            { 
                queryStringParameters.Add(new KeyValuePair<string, string>("priceChannel", productAdditionalParameters.PriceChannel.ToString()));
            }
            if (productAdditionalParameters.PriceCountry != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("priceCountry", productAdditionalParameters.PriceCountry.ToString()));
            }
            if (productAdditionalParameters.PriceCurrency != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("priceCurrency", productAdditionalParameters.PriceCurrency.ToString()));
            }
            if (productAdditionalParameters.PriceCustomerGroup != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("priceCustomerGroup", productAdditionalParameters.PriceCustomerGroup.ToString()));
            }
            if (productAdditionalParameters.Staged != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("staged", productAdditionalParameters.Staged.ToString()));
            }
            return queryStringParameters;
        }
    }
}
