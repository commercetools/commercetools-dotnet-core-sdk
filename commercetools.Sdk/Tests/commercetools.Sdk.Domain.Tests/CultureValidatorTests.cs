using commercetools.Sdk.Domain.Validation;
using System;
using Xunit;

namespace commercetools.Sdk.Domain.Tests
{
    public class CultureValidatorTests
    {
        [Fact]
        public void IsValidCultureName()
        {
            ICultureValidator cultureValidator = new CultureValidator();
            string cultureName = "en-US";
            bool result = cultureValidator.IsCultureValid(cultureName);
            Assert.True(result);
        }

        [Fact]
        public void IsValidCultureTwoLetterName()
        {
            ICultureValidator cultureValidator = new CultureValidator();
            string cultureName = "en";
            bool result = cultureValidator.IsCultureValid(cultureName);
            Assert.True(result);
        }
    }
}
