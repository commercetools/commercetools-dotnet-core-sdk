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

        [JsonProperty(PropertyName = "customerNumber")]
        public string CustomerNumber { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "anonymousCartId")]
        public string AnonymousCartId { get; set; }

        [JsonProperty(PropertyName = "anonymousId")]
        public string AnonymousId { get; set; }

        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "vatId")]
        public string VatId { get; set; }

        [JsonProperty(PropertyName = "isEmailVerified")]
        public bool IsEmailVerified { get; set; }

        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; set; }

        [JsonProperty(PropertyName = "addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty(PropertyName = "defaultBillingAddressId")]
        public string DefaultBillingAddressId { get; set; }

        [JsonProperty(PropertyName = "defaultShippingAddressId")]
        public string DefaultShippingAddressId { get; set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; set; }

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
