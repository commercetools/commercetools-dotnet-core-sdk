using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class SetAddressCustomTypeUpdateAction : UpdateAction<Channel>
    {
        public string Action => "setAddressCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}