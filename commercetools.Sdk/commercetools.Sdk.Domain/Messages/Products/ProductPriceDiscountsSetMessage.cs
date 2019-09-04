using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductPriceDiscountsSet")]
    public class ProductPriceDiscountsSetMessage : Message<Product>
    {
        public List<ProductPriceDiscountsSetUpdatedPrice> UpdatedPrices { get; set;}
    }
}
