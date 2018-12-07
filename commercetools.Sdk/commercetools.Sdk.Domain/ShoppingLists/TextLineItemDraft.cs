namespace commercetools.Sdk.Domain.ShoppingLists
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TextLineItemDraft : IDraft<TextLineItem>
    {
        [Required]
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public double Quantity { get; set; }
        public CustomFields Custom { get; set; }
        public DateTime AddedAt { get; set; }
    }
}