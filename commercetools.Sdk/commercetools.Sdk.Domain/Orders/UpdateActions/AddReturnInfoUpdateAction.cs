using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class AddReturnInfoUpdateAction : OrderUpdateAction
    {
        public override string Action => "addReturnInfo";
        public DateTime? ReturnDate { get; set; }
        public string ReturnTrackingId { get; set; }
        [Required]
        public List<ReturnItemDraft> Items { get; set; }
    }
}
