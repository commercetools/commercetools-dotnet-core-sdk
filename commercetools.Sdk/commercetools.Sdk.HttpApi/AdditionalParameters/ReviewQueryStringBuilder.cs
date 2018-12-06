using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class ReviewQueryStringBuilder : IQueryStringRequestBuilder<Review>
    {
        public List<KeyValuePair<string, string>> GetQueryStringParameters(IAdditionalParameters<Review> additionalParameters)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            ReviewAdditionalParameters productAdditionalParameters = additionalParameters as ReviewAdditionalParameters;
            if (productAdditionalParameters == null)
            {
                return queryStringParameters;
            }

            if (productAdditionalParameters.DataErasure != null)
            {
                queryStringParameters.Add(new KeyValuePair<string, string>("dataErasure", productAdditionalParameters.DataErasure.ToString()));
            }

            return queryStringParameters;
        }
    }
}
