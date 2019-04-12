using commercetools.Sdk.Domain.Validation;
using System;
using Xunit;

namespace commercetools.Sdk.Domain.Tests
{
    public class CountryValidatorTests
    {
        [Fact]
        public void IsValidCountryName()
        {
            ICountryValidator validator = new CountryValidator();
            string name = "US";
            bool result = validator.IsCountryValid(name);
            Assert.True(result);
        }

        [Fact]
        public void IsValidCountryNameInvalidInput()
        {
            ICountryValidator validator = new CountryValidator();
            string name = "USD";
            bool result = validator.IsCountryValid(name);
            Assert.False(result);
        }
    }
}
