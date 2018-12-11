using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Products;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi.UploadImageParameters
{
    public class UploadProductImageParametersBuilder : IUploadImageParametersBuilder
    {
        public bool CanBuild(Type type)
        {
            return type == typeof(UploadProductImageParameters);
        }

        public List<KeyValuePair<string, string>> GetUploadImageParameters<T>(IUploadImageParameters<T> uploadParameters)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            UploadProductImageParameters productImageParameters = uploadParameters as UploadProductImageParameters;
            if (productImageParameters == null)
            {
                return parameters;
            }

            if (productImageParameters.Filename != null)
            {
                parameters.Add(new KeyValuePair<string, string>("filename", productImageParameters.Filename));
            }

            if (productImageParameters.Sku != null)
            {
                parameters.Add(new KeyValuePair<string, string>("sku", productImageParameters.Sku));
            }

            if (productImageParameters.Variant != null)
            {
                parameters.Add(new KeyValuePair<string, string>("variant", productImageParameters.Variant.ToString()));
            }

            if (productImageParameters.Staged != null)
            {
                parameters.Add(new KeyValuePair<string, string>("staged", productImageParameters.Staged.ToString()));
            }

            return parameters;
        }
    }
}
