namespace commercetools.Sdk.Domain.CartDiscounts
{
    [TypeMarker("multiBuyLineItems")]
    public class MultiBuyLineItemTarget : CartDiscountTarget
    {
        public string Predicate { get; set; }
       
        public int TriggerQuantity { get; set; }

        public int DiscountedQuantity { get; set; }

        public int MaxOccurrence { get; set; }
        
        public SelectionMode SelectionMode { get; set; }
    }
}