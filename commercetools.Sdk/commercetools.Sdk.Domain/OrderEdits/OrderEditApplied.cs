using System;

namespace commercetools.Sdk.Domain.OrderEdits
{
    [TypeMarker("Applied")]
    public class OrderEditApplied : OrderEditResult
    {
        public DateTime AppliedAt { get; set; }
        
        public OrderExcerpt ExcerptBeforeEdit { get; set; }
        
        public OrderExcerpt ExcerptAfterEdit { get; set; }
    }
}