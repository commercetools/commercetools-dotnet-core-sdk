using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Projects
{
    [TypeMarker("CartClassification")]
    public class CartClassificationShippingRateInputType : ShippingRateInputType
    {
        public List<LocalizedEnumValue> Values { get; set; }
    }
}
