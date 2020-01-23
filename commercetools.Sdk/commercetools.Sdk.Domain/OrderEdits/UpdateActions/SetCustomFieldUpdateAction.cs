using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.UpdateActions
{
    public class SetCustomFieldUpdateAction : UpdateAction<OrderEdit>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}