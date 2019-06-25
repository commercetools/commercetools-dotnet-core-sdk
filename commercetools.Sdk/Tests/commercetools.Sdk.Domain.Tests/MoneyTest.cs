using NodaMoney;
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
            var nm = m.ToNodaMoney();
            Assert.IsType<NodaMoney.Money>(nm);
        }

        [Fact]
        public void ConvertHighPrecisionMoney()
        {
            var m = new HighPrecisionMoney() { CurrencyCode = "EUR", PreciseAmount = 12345678, FractionDigits = 4 };
            var nm = m.ToNodaMoney();
            Assert.IsType<NodaMoney.Money>(nm);
            Assert.Equal(1234.5678M, nm.Amount);
        }

        [Fact]
        public void ConvertToHighPrecisionMoney()
        {
            var builder = new CurrencyBuilder("EUR", "HighPrecision");
            builder.LoadDataFromCurrency(Currency.FromCode("EUR"));
            builder.DecimalDigits = 4;

            var nm = new NodaMoney.Money(1234.5678m, builder.Build());
            var m = nm.ToCtpHighPrecisionMoney();
            Assert.IsType<HighPrecisionMoney>(m);
            Assert.Equal(12345678, m.PreciseAmount);
            Assert.Equal(4, m.FractionDigits);
            Assert.Equal("EUR", m.CurrencyCode);
        }

        [Fact]
        public void ConvertToMoney()
        {
            var builder = new CurrencyBuilder("EUR", "HighPrecision");
            builder.LoadDataFromCurrency(Currency.FromCode("EUR"));
            builder.DecimalDigits = 4;

            var nm = new NodaMoney.Money(1234.5678m, builder.Build());
            var m = nm.ToCtpMoney();
            Assert.IsType<Money>(m);
            Assert.Equal(123457, m.CentAmount);
            Assert.Equal("EUR", m.CurrencyCode);
        }
    }
}
