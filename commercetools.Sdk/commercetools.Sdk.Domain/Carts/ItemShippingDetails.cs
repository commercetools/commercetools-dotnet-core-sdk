using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Carts
{
    public class ItemShippingDetails
    {
        public List<ItemShippingTarget> Targets { get; set; }
        public bool Valid { get; set; }
    }
}