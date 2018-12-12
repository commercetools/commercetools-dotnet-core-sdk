using System.Linq;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("relative")]
    public class RelativeProductDiscountValue : ProductDiscountValue
    {
        public int Permyriad { get; set; }
    }
}