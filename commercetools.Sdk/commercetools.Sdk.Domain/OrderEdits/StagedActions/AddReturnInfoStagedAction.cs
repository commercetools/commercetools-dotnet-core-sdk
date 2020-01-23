using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class AddReturnInfoStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "addReturnInfo";
        public DateTime? ReturnDate { get; set; }
        public string ReturnTrackingId { get; set; }
        [Required]
        public List<ReturnItemDraft> Items { get; set; }
    }
}
