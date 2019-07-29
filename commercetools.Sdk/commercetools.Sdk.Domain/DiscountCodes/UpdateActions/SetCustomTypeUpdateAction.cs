using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setCustomType";
        public IReference<Type> Type { get; set; }
        public Fields Fields { get; set; }
    }
}
