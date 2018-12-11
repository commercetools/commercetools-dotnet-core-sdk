using System;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class GetMatchingProductDiscountCommand : Command<ProductDiscount>
    {
        public GetMatchingProductDiscountCommand(Price price)
        {
            this.Price = price;
        }

        public Guid ProductId { get; set; }

        public int VariantId { get; set; }

        public bool Staged { get; set; }

        public Price Price { get; }

        public override System.Type ResourceType => typeof(ProductDiscount);
    }
}