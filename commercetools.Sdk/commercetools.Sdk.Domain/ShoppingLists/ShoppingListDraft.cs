namespace commercetools.Sdk.Domain.ShoppingLists
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Customers;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class ShoppingListDraft : IDraft<ShoppingList>
    {
        public string Key { get; set; }
        [Slug]
        public LocalizedString Slug { get; set; }
        [Required]
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public ResourceIdentifier Customer { get; set; }
        public string AnonymousId { get; set; }
        public List<LineItemDraft> LineItems { get; set; }
        public List<TextLineItem> TextLineItems { get; set; }
        public CustomFields Custom { get; set; }
        public int DeleteDaysAfterLastModification { get; set; }
    }
}
