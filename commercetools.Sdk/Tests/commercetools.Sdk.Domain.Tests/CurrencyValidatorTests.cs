using commercetools.Sdk.Domain.Validation;
using System;
using Xunit;

namespace commercetools.Sdk.Domain.Tests
{
    public class CurrencyValidatorTests
    {
        [Fact]
        public void IsValidCurrency()
        {
            ICurrencyValidator validator = new CurrencyValidator();
            string name = "USD";
            bool result = validator.IsCurrencyValid(name);
            Assert.True(result);
        }

        [Fact]
        public void IsValidCurrencyInvalidInput()
        {
            ICurrencyValidator validator = new CurrencyValidator();
            string name = "USX";
            bool result = validator.IsCurrencyValid(name);
            Assert.False(result);
        }
    }
}
