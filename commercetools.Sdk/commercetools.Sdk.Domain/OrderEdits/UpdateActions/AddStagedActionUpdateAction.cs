using System.Collections.Generic;

namespace commercetools.Sdk.Domain.OrderEdits.UpdateActions
{
    public class AddStagedActionUpdateAction : UpdateAction<OrderEdit>
    {
        public string Action => "addStagedAction";
        public StagedOrderUpdateAction StagedAction { get; set; }
    }
}