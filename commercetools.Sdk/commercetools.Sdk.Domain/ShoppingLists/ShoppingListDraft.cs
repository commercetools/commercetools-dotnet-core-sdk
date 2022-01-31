using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Stores;

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
        public IReference<Customer> Customer { get; set; }
        public IReferenceable<Store> Store { get; set; }
        public string AnonymousId { get; set; }
        public List<LineItemDraft> LineItems { get; set; }
        public List<TextLineItemDraft> TextLineItems { get; set; }
        public CustomFieldsDraft Custom { get; set; }
        public int? DeleteDaysAfterLastModification { get; set; }
    }
}
