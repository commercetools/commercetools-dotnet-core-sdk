using commercetools.Sdk.Domain.CartDiscounts;

namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<Channel>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}
