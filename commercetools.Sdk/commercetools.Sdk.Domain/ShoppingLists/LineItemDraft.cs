namespace commercetools.Sdk.Domain.ShoppingLists
{
    using System;

    public class LineItemDraft : IDraft<LineItem>
    {
        public string ProductId { get; set; }
        public int VariantId { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public CustomFieldsDraft Custom { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
