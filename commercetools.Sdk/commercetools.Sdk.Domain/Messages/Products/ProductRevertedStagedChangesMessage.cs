using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductRevertedStagedChanges")]
    public class ProductRevertedStagedChangesMessage : Message<Product>
    {
        public List<string> RemovedImageUrls { get; set; }
    }
}
