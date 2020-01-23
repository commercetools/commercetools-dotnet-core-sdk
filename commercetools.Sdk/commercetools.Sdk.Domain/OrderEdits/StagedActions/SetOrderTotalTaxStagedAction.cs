using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetOrderTotalTaxStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setOrderTotalTax";
        public Money ExternalTotalGross { get; set; }
        public List<TaxPortion> ExternalTaxPortions { get; set; }
    }
}
