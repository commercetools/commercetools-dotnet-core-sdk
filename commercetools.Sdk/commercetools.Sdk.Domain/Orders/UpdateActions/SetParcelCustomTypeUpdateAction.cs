using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetParcelCustomTypeUpdateAction : OrderUpdateAction
    {
        public override string Action => "setParcelCustomType";
        [Required]
        public string ParcelId { get; set; }
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}