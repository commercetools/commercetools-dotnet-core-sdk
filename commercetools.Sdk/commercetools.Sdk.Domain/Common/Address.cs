namespace commercetools.Sdk.Domain
{
    public class Address
    {
        public string Id { get; set; }

        // TODO Add validation
        public string Key { get; set; }

        public string Title { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string AdditionalStreetInfo { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string State { get; set; }

        // Add validation
        public string Country { get; set; }

        public string Company { get; set; }
        public string Department { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }
        public string POBox { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string AdditionalAddressInfo { get; set; }
        public string ExternalId { get; set; }

        public override string ToString()
        {
            return "Address{" +
                   "country=" + Country +
                   ", key=" + Key +
                   ", title=" + Title +
                   ", salutation=" + Salutation +
                   ", firstName=" + FirstName +
                   ", lastName=" + LastName +
                   ", streetName=" + StreetName +
                   ", streetNumber=" + StreetNumber +
                   ", additionalStreetInfo=" + AdditionalStreetInfo +
                   ", postalCode=" + PostalCode +
                   ", city=" + City +
                   ", region=" + Region +
                   ", state=" + State +
                   ", company=" + Company +
                   ", department=" + Department +
                   ", building=" + Building +
                   ", apartment=" + Apartment +
                   ", poBox=" + POBox +
                   ", phone=" + Phone +
                   ", mobile=" + Mobile +
                   ", email=" + Email +
                   ", additionalAddressInfo=" + AdditionalAddressInfo +
                   ", fax=" + Fax +
                   ", externalId=" + ExternalId +
                   "}";
        }
    }
}
