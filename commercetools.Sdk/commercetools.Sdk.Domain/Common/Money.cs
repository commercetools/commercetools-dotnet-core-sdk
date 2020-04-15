using System.Globalization;
using System.Linq;

namespace commercetools.Sdk.Domain
{
    using System;

    [MoneyType(Type = MoneyType.CentPrecision)]
    public class Money : BaseMoney
    {
        public override MoneyType Type => MoneyType.CentPrecision;

        public override decimal AmountToDecimal()
        {
            return (decimal)CentAmount / 100M;
        }

        [Obsolete("will be replaced by more resilient implementation")]
        public static Money Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
            var splitInput = input.Split(' ');
            if (splitInput.Length != 2)
            {
                throw new ArgumentException();
            }
            var amount = decimal.Parse(splitInput[0]);
            var currencyCode = splitInput[1];
            return new Money() { CentAmount = (int)amount * 100, CurrencyCode = currencyCode };
        }

        public static Money FromDecimal(string currencyCode, decimal value, MidpointRounding midpointRounding = MidpointRounding.ToEven)
        {
            var amount = Math.Round(value * 100M, 0, midpointRounding);
            return new Money
            {
                CurrencyCode = currencyCode,
                CentAmount = (long)amount
            };
        }
    }
}
