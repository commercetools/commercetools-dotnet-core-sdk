using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetShippingAddressAndShippingMethodStagedAction : IStagedOrderUpdateAction
    {
        public string Action => "updateSyncInfo";
        public Address Address { get; set; }
        public IReference<ShippingMethod> ShippingMethod { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}
