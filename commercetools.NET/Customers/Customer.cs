using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Customers
{
    /// <summary>
    /// A customer is a person purchasing products. Carts, Orders and Reviews can be associated to a customer. 
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-customers.html#customer"/>
    public class Customer
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; private set; }

        [JsonProperty(PropertyName = "customerNumber")]
        public string CustomerNumber { get; private set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "lastModifiedAt")]
        public DateTime? LastModifiedAt { get; private set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; private set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; private set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; private set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; private set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; private set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; private set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime? DateOfBirth { get; private set; }

        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; private set; }

        [JsonProperty(PropertyName = "vatId")]
        public string VatId { get; private set; }

        [JsonProperty(PropertyName = "addresses")]
        public List<Address> Addresses { get; private set; }

        [JsonProperty(PropertyName = "defaultShippingAddressId")]
        public string DefaultShippingAddressId { get; private set; }

        [JsonProperty(PropertyName = "shippingAddressIds")]
        public List<string> ShippingAddressIds { get; private set; }

        [JsonProperty(PropertyName = "defaultBillingAddressId")]
        public string DefaultBillingAddressId { get; private set; }

        [JsonProperty(PropertyName = "billingAddressIds")]
        public List<string> BillingAddressIds { get; private set; }

        [JsonProperty(PropertyName = "isEmailVerified")]
        public bool IsEmailVerified { get; private set; }

        [JsonProperty(PropertyName = "externalId")]
        public string ExternalId { get; private set; }

        [JsonProperty(PropertyName = "customerGroup")]
        public Reference CustomerGroup { get; private set; }

        [JsonProperty(PropertyName = "custom")]
        public CustomFields.CustomFields Custom { get; private set; }

        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Customer(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Version = data.version;
            this.CustomerNumber = data.customerNumber;
            this.CreatedAt = data.createdAt;
            this.LastModifiedAt = data.lastModifiedAt;
            this.Email = data.email;
            this.Password = data.password;
            this.FirstName = data.firstName;
            this.LastName = data.lastName;
            this.MiddleName = data.middleName;
            this.Title = data.title;
            this.DateOfBirth = data.dateOfBirth;
            this.CompanyName = data.companyName;
            this.VatId = data.vatId;
            this.Addresses = Helper.GetListFromJsonArray<Address>(data.addresses);
            this.DefaultShippingAddressId = data.defaultShippingAddressId;
            this.ShippingAddressIds = Helper.GetListFromJsonArray<string>(data.shippingAddressIds);
            this.DefaultBillingAddressId = data.defaultBillingAddressId;
            this.BillingAddressIds = Helper.GetListFromJsonArray<string>(data.billingAddressIds);
            this.IsEmailVerified = data.isEmailVerified;
            this.ExternalId = data.externalId;
            this.CustomerGroup = new Reference(data.customerGroup);
            this.Custom = new CustomFields.CustomFields(data.custom);
            this.Locale = data.locale;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Customer customer = obj as Customer;

            if (customer == null)
            {
                return false;
            }

            return customer.Id.Equals(this.Id);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
