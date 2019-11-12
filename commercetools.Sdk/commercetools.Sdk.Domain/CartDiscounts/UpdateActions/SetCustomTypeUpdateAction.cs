using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Types;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setCustomType";
        public IReference<Type> Type { get; set; }
        public Fields Fields { get; set; }
    }
}
