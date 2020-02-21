using System.Collections.Generic;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetOrderTotalTaxStagedAction : IStagedOrderUpdateAction
    {
        public string Action => "setOrderTotalTax";
        public Money ExternalTotalGross { get; set; }
        public List<TaxPortion> ExternalTaxPortions { get; set; }
    }
}
