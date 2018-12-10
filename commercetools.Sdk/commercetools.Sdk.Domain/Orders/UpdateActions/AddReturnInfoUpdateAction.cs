using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class AddReturnInfoUpdateAction : UpdateAction<Order>
    {
        public string Action => "addReturnInfo";
        public DateTime ReturnDate { get; set; }
        public string ReturnTrackingId { get; set; }
        [Required]
        public List<ReturnItemDraft> Items { get; set; }
    }
}