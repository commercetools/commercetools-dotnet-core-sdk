using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.CartDiscounts;

namespace commercetools.Sdk.Domain.Channels.UpdateActions
{
    public class SetCustomFieldUpdateAction : UpdateAction<Channel>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
