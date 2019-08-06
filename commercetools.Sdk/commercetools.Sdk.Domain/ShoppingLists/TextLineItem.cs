namespace commercetools.Sdk.Domain.ShoppingLists
{
    using System;

    public class TextLineItem
    {
        public string Id { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public long Quantity { get; set; }
        public CustomFields Custom { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
