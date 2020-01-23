using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.TaxCategories;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetShippingAddressAndCustomShippingMethodStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setShippingAddressAndCustomShippingMethod";
        
        public Address Address { get; set; }
        
        [Required]
        public string ShippingMethodName { get; set; }

        public IReference<TaxCategory> TaxCategory { get; set; }

        public ExternalTaxRateDraft ExternalTaxRateDraft { get; set; }
    }
}