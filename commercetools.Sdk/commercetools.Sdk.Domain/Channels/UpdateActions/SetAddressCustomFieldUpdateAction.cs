using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class SetAddressCustomFieldUpdateAction : UpdateAction<Channel>
    {
        public string Action => "setAddressCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}