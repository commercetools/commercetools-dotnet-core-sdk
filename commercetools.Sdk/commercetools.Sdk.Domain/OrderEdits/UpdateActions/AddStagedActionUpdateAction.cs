using System.Collections.Generic;

namespace commercetools.Sdk.Domain.OrderEdits.UpdateActions
{
    public class AddStagedActionUpdateAction : UpdateAction<OrderEdit>
    {
        public string Action => "addStagedAction";
        public IStagedOrderUpdateAction StagedAction { get; set; }
    }
}