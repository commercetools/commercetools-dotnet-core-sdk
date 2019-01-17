using System;

namespace commercetools.Sdk.Domain.Customers
{
    public class CustomerToken : Token<Customer>
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Value { get; set; }
    }
}
