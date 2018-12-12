using System.Linq;

namespace commercetools.Sdk.Domain
{
    public abstract class ProductDiscountValue
    {
        public string Type
        {
            get => this.GetType().GetTypeMarkerAttributeValue();
        }
    }
}