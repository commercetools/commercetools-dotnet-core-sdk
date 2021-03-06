﻿using System.Collections.Generic;

namespace commercetools.Sdk.Domain.OrderEdits.UpdateActions
{
    public class SetStagedActionsUpdateAction : UpdateAction<OrderEdit>
    {
        public string Action => "setStagedActions";
        public List<IStagedOrderUpdateAction> StagedActions { get; set; }
    }
}