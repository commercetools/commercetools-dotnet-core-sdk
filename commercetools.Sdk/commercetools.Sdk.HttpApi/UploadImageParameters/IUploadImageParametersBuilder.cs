using System.Collections.Generic;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.HttpApi.UploadImageParameters
{
    public interface IUploadImageParametersBuilder : IParametersBuilder
    {
        List<KeyValuePair<string, string>> GetUploadImageParameters<T>(IUploadImageParameters<T> uploadParameters);
    }
}