using Xunit;

namespace commercetools.Sdk.Domain.Tests
{
    public class MoneyTest
    {
        [Fact]
        public void MoneyToDecimal()
        {
            var m = new Money() { CurrencyCode = "EUR", CentAmount = 123456 };
            decimal expect = 1234.56M;
            Assert.Equal(expect, m.AmountToDecimal());
        }

        [Fact]
        public void HighPrecisionMoneyToDecimal()
        {
            var m = new HighPrecisionMoney() { CurrencyCode = "EUR", PreciseAmount = 12345678, FractionDigits = 4 };
            decimal expect = 1234.5678M;
            Assert.Equal(expect, m.AmountToDecimal());
        }

        [Fact]
        public void ConvertMoney()
        {
            var m = new Money() { CurrencyCode = "EUR", FractionDigits = 4 };
            var nm = m.Convert();
            Assert.IsType<NodaMoney.Money>(nm);
        }

        [Fact]
        public void ConvertHighPrecisionMoney()
        {
            var m = new HighPrecisionMoney() { CurrencyCode = "EUR", PreciseAmount = 12345678, FractionDigits = 4 };
            var nm = m.Convert();
            Assert.IsType<NodaMoney.Money>(nm);
            Assert.Equal(1234.5678M, nm.Amount);
        }
    }
}
