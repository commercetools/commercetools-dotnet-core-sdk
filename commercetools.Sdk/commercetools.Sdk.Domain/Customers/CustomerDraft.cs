using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Stores;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Customers
{
    public class CustomerDraft : IDraft<Customer>
    {
        public string CustomerNumber { get; set; }
        public string Key { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public string Salutation { get; set; }
        public string AnonymousId { get; set; }
        public string AnonymousCartId { get; set; }
        public string DateOfBirth { get; set; }
        public string CompanyName { get; set; }
        public string VatId { get; set; }
        public List<Address> Addresses { get; set; }
        public int? DefaultShippingAddress { get; set; }
        public List<int> ShippingAddresses { get; set; }
        public int? DefaultBillingAddress { get; set; }
        public List<int> BillingAddresses { get; set; }
        public bool? IsEmailVerified { get; set; }
        public string ExternalId { get; set; }
        public IReference<CustomerGroup> CustomerGroup { get; set; }
        public CustomFieldsDraft Custom { get; set; }
        [Language]
        public string Locale { get; set; }
        
        public List<ResourceIdentifier<Store>> Stores { get; set; }
    }
}