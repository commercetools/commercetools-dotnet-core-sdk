using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Orders
{
    public class ReturnInfoDraft
    {
        public List<ReturnItemDraft> Items { get; set; }
        public string ReturnTrackingId { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}