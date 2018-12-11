using System;
using System.IO;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class UploadProductImageCommand : UploadImageCommand<Product>
    {
        public UploadProductImageCommand(Guid id, Stream image, string contentType)
            : base(id, image, contentType)
        {
        }
    }
}
