using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers
{
    /// <summary>
    /// API representation for creating a new Customer.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#customerdraft"/>
    public class CustomerDraft
    {
        #region Properties

        /// <summary>
        /// String that uniquely identifies a customer.
        /// </summary>
        /// <remarks>
        /// It can be used to create more human-readable (in contrast to ID) identifier for the customer. It should be unique across a project. Once it’s set it cannot be changed.
        /// </remarks>
        [JsonProperty(PropertyName = "customerNumber")]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// Stored in given case. For the uniqueness check, it is treated as case-insensitive.
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Middle Name
        /// </summary>
        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Identifies a single cart that will be assigned to the new customer account.
        /// </summary>
        [JsonProperty(PropertyName = "anonymousCartId")]
        public string AnonymousCartId { get; set; }

        /// <summary>
        /// Identifies carts and orders belonging to an anonymous session that will be assigned to the new customer account.
        /// </summary>
        [JsonProperty(PropertyName = "anonymousId")]
        public string AnonymousId { get; set; }

        /// <summary>
        /// External Id
        /// </summary>
        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Date Of Birth
        /// </summary>
        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Company Name
        /// </summary>
        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Vat Id
        /// </summary>
        [JsonProperty(PropertyName = "vatId")]
        public string VatId { get; set; }

        /// <summary>
        /// Is Email Verified
        /// </summary>
        [JsonProperty(PropertyName = "isEmailVerified")]
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Reference to a CustomerGroup
        /// </summary>
        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; set; }

        /// <summary>
        /// Sets the ID of each address to be unique in the addresses list.
        /// </summary>
        [JsonProperty(PropertyName = "addresses")]
        public List<Address> Addresses { get; set; }

        /// <summary>
        /// The index of the address in the addresses array. The defaultBillingAddressId of the customer will be set to the ID of that address.
        /// </summary>
        [JsonProperty(PropertyName = "defaultBillingAddress")]
        public int? DefaultBillingAddress { get; set; }

        /// <summary>
        /// The indices of the addresses in the addresses array. The billingAddressIds of the customer will be set to the IDs of that addresses.
        /// </summary>
        [JsonProperty(PropertyName = "billingAddresses")]
        public List<int> BillingAddresses { get; set; }

        /// <summary>
        /// The index of the address in the addresses array. The defaultShippingAddressId of the customer will be set to the ID of that address.
        /// </summary>
        [JsonProperty(PropertyName = "defaultShippingAddress")]
        public int? DefaultShippingAddress { get; set; }

        /// <summary>
        /// The indices of the addresses in the addresses array. The shippingAddressIds of the Customer will be set to the IDs of that addresses.
        /// </summary>
        [JsonProperty(PropertyName = "shippingAddresses")]
        public List<int> ShippingAddresses { get; set; }

        /// <summary>
        /// The custom fields.
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; set; }

        /// <summary>
        /// String conforming to IETF language tag.
        /// </summary>
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomerDraft(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        #endregion
    }
}
