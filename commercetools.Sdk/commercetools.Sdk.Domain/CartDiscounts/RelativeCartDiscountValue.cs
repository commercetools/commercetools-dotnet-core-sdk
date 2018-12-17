using System.Linq;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("relative")]
    public class RelativeCartDiscountValue : CartDiscountValue
    {
        public int Permyriad { get; set; }
    }
}