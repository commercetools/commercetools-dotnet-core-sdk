using commercetools.Sdk.Domain.Customers.UpdateActions;

namespace commercetools.Sdk.Domain.Messages
{
    public class UserProvidedIdentifiers
    {
        public string Key { get; set; }

        public string ExternalId { get; set; }

        public string OrderNumber { get; set; }

        public string CustomerNumber { get; set; }

        public string Sku { get; set; }

        public LocalizedString Slug { get; set; }
    }
}