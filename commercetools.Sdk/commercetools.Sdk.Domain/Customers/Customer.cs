using commercetools.Sdk.Domain.CustomerGroups;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Customers
{
    [Endpoint("customers")]
    [ResourceType(ReferenceTypeId.Customer)]
    [CustomizeSerializationMarker]
    public class Customer : Resource<Customer>, IKeyReferencable<Customer>,IInStoreUsable, ISignupable
    {
        public string CustomerNumber { get; set; }
        public string Key { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public string Salutation { get; set; }
        [AsDateOnly]
        public DateTime? DateOfBirth { get; set; }
        public string CompanyName { get; set; }
        public string VatId { get; set; }
        public List<Address> Addresses { get; set; }
        public string DefaultShippingAddressId { get; set; }
        public List<string> ShippingAddressIds { get; set; }
        public string DefaultBillingAddressId { get; set; }
        public List<string> BillingAddressIds { get; set; }
        public bool IsEmailVerified { get; set; }
        public string ExternalId { get; set; }
        public Reference<CustomerGroup> CustomerGroup { get; set; }
        public CustomFields Custom { get; set; }
        public string Locale { get; set; }

        public List<KeyReference<Store>> Stores { get; set; }
        public AuthenticationMode AuthenticationMode { get; set; }
    }
}
