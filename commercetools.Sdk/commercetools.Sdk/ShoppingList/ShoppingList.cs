using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class ShoppingList
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public LocalizedString Slug { get; set; }
        public LocalizedString Name { get; set; }
        public Reference<Customer> Customer { get; set; }
        public string AnonymousId { get; set; }
        public List<LineItem> LineItems { get; set; }
        public List<TextLineItem> TextLineItems { get; set; }
        public CustomFields Custom { get; set; }
        public int DeleteDaysAfterLastModification { get; set; }
    }
}
