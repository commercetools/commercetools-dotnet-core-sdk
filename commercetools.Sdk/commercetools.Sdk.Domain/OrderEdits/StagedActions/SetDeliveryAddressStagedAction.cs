using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetDeliveryAddressStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setDeliveryAddress";
        [Required]
        public string DeliveryId { get; set; }
        public Address Address { get; set; }
    }
}