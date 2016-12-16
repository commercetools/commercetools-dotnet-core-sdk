using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// An Address is a JSON string representation of a postal address.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-types.html#address"/>
    public class Address
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "salutation")]
        public string Salutation { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "streetName")]
        public string StreetName { get; set; }

        [JsonProperty(PropertyName = "streetNumber")]
        public string StreetNumber { get; set; }

        [JsonProperty(PropertyName = "additionalStreetInfo")]
        public string AdditionalStreetInfo { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; }

        [JsonProperty(PropertyName = "department")]
        public string Department { get; set; }

        [JsonProperty(PropertyName = "building")]
        public string Building { get; set; }

        [JsonProperty(PropertyName = "apartment")]
        public string Apartment { get; set; }

        [JsonProperty(PropertyName = "pOBox")]
        public string POBox { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "additionalAddressInfo")]
        public string AdditionalAddressInfo { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public Address() 
        {
        }

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Address(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Id = data.id;
            this.Title = data.title;
            this.Salutation = data.salutation;
            this.FirstName = data.firstName;
            this.LastName = data.lastName;
            this.StreetName = data.streetName;
            this.StreetNumber = data.streetNumber;
            this.AdditionalStreetInfo = data.additionalStreetInfo;
            this.PostalCode = data.postalCode;
            this.City = data.city;
            this.Region = data.region;
            this.State = data.state;
            this.Country = data.country;
            this.Company = data.company;
            this.Department = data.department;
            this.Building = data.building;
            this.Apartment = data.apartment;
            this.POBox = data.pOBox;
            this.Phone = data.phone;
            this.Mobile = data.mobile;
            this.Email = data.email;
            this.AdditionalAddressInfo = data.additionalAddressInfo;
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
            Address address = obj as Address;

            if (address == null)
            {
                return false;
            }

            return object.Equals(address.Id, this.Id);
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
