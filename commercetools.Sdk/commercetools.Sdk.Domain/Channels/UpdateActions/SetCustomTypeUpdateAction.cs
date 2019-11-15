using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Types;

namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<Channel>
    {
        public string Action => "setCustomType";
        public IReference<Type> Type { get; set; }
        public Fields Fields { get; set; }
    }
}
