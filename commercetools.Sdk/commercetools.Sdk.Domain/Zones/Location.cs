using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Zones
{
    public class Location
    {
        [Country]
        [Required]
        public string Country { get; set; }
        public string State { get; set; }
    }
}