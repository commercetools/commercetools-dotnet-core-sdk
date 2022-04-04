using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetParcelCustomFieldUpdateAction : OrderUpdateAction
    {
        public override string Action => "setParcelCustomField";
        [Required]
        public string ParcelId { get; set; }
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}