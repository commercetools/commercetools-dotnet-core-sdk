using System;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits
{
    public class OrderExcerpt
    {
        public Money TotalPrice { get; set; }
        
        public TaxedPrice TaxedPrice { get; set; }
        
        public int Version { get; set; }
    }
}