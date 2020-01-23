using System;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.ShippingMethods;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetShippingAddressAndShippingMethodStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "updateSyncInfo";
        public Address Address { get; set; }
        public IReference<ShippingMethod> ShippingMethod { get; set; }
        public ExternalTaxRateDraft ExternalTaxRate { get; set; }
    }
}
